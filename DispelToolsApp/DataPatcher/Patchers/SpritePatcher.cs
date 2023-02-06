using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using DispelTools.GameDataModels.Sprite;
using DispelTools.ImageProcessing;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DispelTools.DataPatcher.Patchers
{
    public class SpritePatcherFactory : IPatcherFactory
    {
        public string PatcherName => "Image patcher (SPR)";

        public string PatchFileFilter => "Image (*.PNG)|*.PNG;*.png|All files (*.*)|*.*";

        public string OutputFileFilter => "sprite (*.SPR)|*.SPR;*.spr|All files (*.*)|*.*";
        public string OutputFileExtension => ".spr";

        public PatcherParams.OptionNames AcceptedOptions { get; } = PatcherParams.OptionNames.KeepBackupFiles | PatcherParams.OptionNames.KeepImageSize;

        public Patcher CreateInstance()
        {
            return new SpritePatcher();
        }
    }

    public class SpritePatchFile : PatchFile
    {
        public SpritePatchFile(string patchFile, string destinationFile, int id, int frame) : base(patchFile, destinationFile)
        {
            Id = id;
            Frame = frame;
        }

        public int Id { get; }
        public int Frame { get; }
    }

    public class SpriteReadProcess : OpenedSpriteFile
    {
        private readonly SpritePatcher patcher;
        public SpriteReadProcess(BinaryReader file, string filename, ColorManagement.ColorMode colorMode, SpritePatcher patcher) : base(file, filename, colorMode)
        {
            this.patcher = patcher;
        }

        public override void Process(SpriteSequence sequence, int imageNumber)
        {
            if (sequence.Animated)
            {
                for (int i = 0; i < sequence.FrameCount; i++)
                {
                    SpriteLoader.ImageInfo frameInfo = sequence.SequenceInfo.FrameInfos[i];
                    patcher.AddPatchMark(frameInfo.ImageStartPosition, imageNumber, i, frameInfo.Width, frameInfo.Height);
                }
            }
            else
            {
                SpriteLoader.ImageInfo frameInfo = sequence.SequenceInfo.FrameInfos[0];
                patcher.AddPatchMark(frameInfo.ImageStartPosition, imageNumber, -1, frameInfo.Width, frameInfo.Height);
            }
        }
    }

    public class SpritePatcher : Patcher
    {
        private readonly SortedDictionary<long, PatchMark> patchMarks = new SortedDictionary<long, PatchMark>();
        private readonly List<SpritePatchFile> patches = new List<SpritePatchFile>();
        private static readonly int imageSizeByteSize = 4 * 3;

        private class PatchMark
        {
            public PatchMark(long filePosition, int height, int width, PatchFile patchFile)
            {
                FilePosition = filePosition;
                Height = height;
                Width = width;
                PatchFile = patchFile;
            }

            public long FilePosition { get; }
            public int Height { get; }
            public int Width { get; }
            public PatchFile PatchFile { get; }
        }

        public override int Count => patches.Count;

        public override void PatchFile(PatcherParams.PatcherOptions options, DetailedProgressReporter workReporter)
        {
            ColorManagement.ColorMode colorMode = ColorManagement.ColorMode.RGB16_565;
            ColorManagement.ColorMode targetColorMode = ColorManagement.ColorMode.RGB16_565;
            ColorManagement sourceColorManagement = ColorManagement.From(colorMode);
            ColorManagement targetColorManagement = ColorManagement.From(targetColorMode);
            string destinationFile = patches[0].DestinationFile;

            //Read sprite data
            using (BinaryReader reader = new BinaryReader(fs.File.Open(destinationFile, FileMode.Open)))
            {
                workReporter.SetTotal((int)reader.BaseStream.Length);
                SpriteReadProcess destinationSpriteFileProcess = new SpriteReadProcess(reader, destinationFile, colorMode, this);
                SpriteFileReader.ProcessThroughFile(destinationSpriteFileProcess, false);
            }

            //Backup data
            string backupFile = destinationFile + ".0bak";//for keeping original file
            if (fs.File.Exists(backupFile))
            {
                backupFile = destinationFile + ".1bak";//for backup of not original file
            }
            string deleteAfter = string.Empty;
            if (!fs.File.Exists(backupFile) && !options.KeepBackupFiles)
            {
                deleteAfter = backupFile;
            }
            fs.File.Copy(destinationFile, backupFile, true);

            //Save changes to file
            using (BinaryReader backup = new BinaryReader(fs.File.Open(backupFile, FileMode.Open)))
            using (BinaryWriter overriden = new BinaryWriter(fs.File.Open(destinationFile, FileMode.Truncate)))
            {
                foreach (KeyValuePair<long, PatchMark> patch in patchMarks)
                {
                    CopyUntil(patch.Key - imageSizeByteSize, backup, overriden); ;
                    workReporter.ReportProgress((int)backup.BaseStream.Position);
                    long lastWritePosition = overriden.BaseStream.Position;

                    try
                    {
                        ApplyImagePatch(overriden, patch.Value, targetColorManagement, options.KeepImageSize);
                        backup.Skip(patch.Value.Height * patch.Value.Width * sourceColorManagement.BytesConsumed + imageSizeByteSize);//skip image bytes
                        workReporter.ReportDetails($"Applied patch: {fs.Path.GetFileName(patch.Value.PatchFile.PatchFileName)}");
                    }
                    catch (Exception e) when (e is IOException || e is ArgumentException)
                    {
                        workReporter.ReportError($"Patch ${patch.Value.PatchFile.PatchFileName}", e.Message);
                        overriden.BaseStream.Position = lastWritePosition;
                    }
                }
                CopyUntil(backup.BaseStream.Length, backup, overriden);
            }
            workReporter.ReportDetails(PatchingStatus.FileCompleted.Create(patchMarks.Count));

            //delete source file
            if (!string.IsNullOrEmpty(deleteAfter))
            {
                fs.File.Delete(deleteAfter);
            }
        }

        private void CopyUntil(long upToSourcePosition, BinaryReader source, BinaryWriter destination)
        {
            if (upToSourcePosition < source.BaseStream.Position)
            {
                throw new ArgumentException("Can't copy backwards. Source is further than desired location");
            }

            long distance = upToSourcePosition - source.BaseStream.Position;

            int packetSize = 10240;
            for (long i = packetSize; i < distance; i += packetSize)
            {
                byte[] bytes = source.ReadBytes(packetSize);
                destination.Write(bytes);
            }
            int lastDistance = (int)(upToSourcePosition - source.BaseStream.Position);
            if (lastDistance > 0)
            {
                byte[] bytes = source.ReadBytes(lastDistance);
                destination.Write(bytes);
            }
        }

        private void ApplyImagePatch(BinaryWriter target, PatchMark patch, ColorManagement targetColorManagement, bool keepImageSize)
        {
            using var image = new MagickImage(patch.PatchFile.PatchFileName);
            var rawRgbImage = new RawRgb(image.Width, image.Height);
            using (var memory = new MemoryStream(rawRgbImage.Bytes, true))
            {
                image.Write(memory, MagickFormat.Rgb);
            }
            if (keepImageSize)
            {
                if (image.Width != patch.Width || image.Height != patch.Height)
                {
                    throw new ArgumentException("Dimensions of image patch does not match to target image");
                }
            }
            target.Write(image.Width);
            target.Write(image.Height);
            target.Write(image.Width * image.Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    byte[] colorBytes = targetColorManagement.ProduceBytes(rawRgbImage.GetPixel(x, y));
                    target.Write(colorBytes);
                }
            }
        }

        public override void Initialize(List<string> patchFiless, string targetFile, DetailedProgressReporter workReporter)
        {
            foreach (string patchFileName in patchFiless)
            {
                string noExtensionName = fs.Path.GetFileNameWithoutExtension(patchFileName);
                string encodedParams = noExtensionName.Substring(noExtensionName.LastIndexOf('.') + 1);
                int frameSeparationIndex = encodedParams.IndexOf("_");

                try
                {
                    SpritePatchFile result;
                    if (frameSeparationIndex < 0)
                    {
                        result = new SpritePatchFile(patchFileName, targetFile, int.Parse(encodedParams), -1);
                    }
                    else
                    {
                        string decodedId = encodedParams.Substring(0, frameSeparationIndex);
                        string decodedFrame = encodedParams.Substring(frameSeparationIndex + 2);
                        result = new SpritePatchFile(patchFileName, targetFile, int.Parse(decodedId), int.Parse(decodedFrame));
                    }
                    patches.Add(result);
                }
                catch (Exception e) when (e is FormatException || e is ArgumentNullException)
                {
                    workReporter.ReportError($"Parsing [{patchFileName}] with encoded params [{encodedParams}]", e.Message);
                }
            }
        }

        internal void AddPatchMark(long filePosition, int id, int frame, int width, int height)
        {
            SpritePatchFile foundPatch = patches.Find(patch => patch.Id == id && patch.Frame == frame);
            if (foundPatch != null)
            {
                patchMarks.Add(filePosition, new PatchMark(filePosition, height, width, foundPatch));
            }
        }

    }
}

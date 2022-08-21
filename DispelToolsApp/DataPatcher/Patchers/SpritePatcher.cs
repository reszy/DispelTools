using DispelTools.Common;
using DispelTools.GameDataModels.Sprite;
using DispelTools.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DispelTools.DataPatcher.Patchers
{
    internal class SpritePatcherFactory : IPatcherFactory
    {
        public string PatcherName => "Image patcher (SPR)";

        public string PatchFileFilter => "Image (*.PNG)|*.PNG;*.png|All files (*.*)|*.*";

        public string OutputFileFilter => "sprite (*.SPR)|*.SPR;*.spr|All files (*.*)|*.*";

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
                    var frameInfo = sequence.SequenceInfo.FrameInfos[i];
                    patcher.AddPatchMark(frameInfo.ImageStartPosition, imageNumber, i, frameInfo.Width, frameInfo.Height);
                }
            }
            else
            {
                var frameInfo = sequence.SequenceInfo.FrameInfos[0];
                patcher.AddPatchMark(frameInfo.ImageStartPosition, imageNumber, -1, frameInfo.Width, frameInfo.Height);
            }
        }
    }

    public class SpritePatcher : Patcher
    {
        private readonly SortedDictionary<long, PatchMark> patchMarks = new SortedDictionary<long, PatchMark>();
        private readonly List<SpritePatchFile> patches = new List<SpritePatchFile>();

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

        public override int Count { get => patches.Count; }

        public override void PatchFile(PatcherParams.OptionNames settings, WorkReporter workReporter)
        {
            var colorMode = ColorManagement.ColorMode.RGB16_565;
            var colorManagement = ColorManagement.From(colorMode);

            var destinationFile = patches[0].DestinationFile;
            using (var reader = new BinaryReader(fs.File.Open(destinationFile, FileMode.Open)))
            {
                var destinationSpriteFileProcess = new SpriteReadProcess(reader, destinationFile, colorMode, this);
                SpriteFileReader.ProcessThroughFile(destinationSpriteFileProcess);
            }
            var backupFile = destinationFile + ".0bak";//for keeping original file
            if (fs.File.Exists(backupFile))
            {
                backupFile = destinationFile + ".1bak";//for backup of not original file
            }
            fs.File.Copy(destinationFile, backupFile, true);
            using (var backup = new BinaryReader(fs.File.Open(backupFile, FileMode.Open)))
            using (var overriden = new BinaryWriter(fs.File.Open(destinationFile, FileMode.Truncate)))
            {
                foreach (var patch in patchMarks)
                {
                    CopyUntil(patch.Key, backup, overriden);
                    using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(patch.Value.PatchFile.PatchFileName))
                    {
                        if (image.Width != patch.Value.Width || image.Height != patch.Value.Height)
                        {
                            if (settings.HasFlag(PatcherParams.OptionNames.KeepImageSize))
                            {
                                throw new ArgumentException("Dimensions of image patch does not match to target image");
                                //workReporter.
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        for (int y = 0; y < image.Height; y++)
                        {
                            for (int x = 0; x < image.Width; x++)
                            {
                                var colorBytes = colorManagement.ProduceBytes(image.GetPixel(x, y));
                                overriden.Write(colorBytes);
                            }
                        }
                    }
                    backup.Skip(patch.Value.Height * patch.Value.Width * colorManagement.BytesConsumed);//skip image bytes
                }
                CopyUntil(backup.BaseStream.Length, backup, overriden);
            }
        }

        private void CopyUntil(long upToSourcePosition, BinaryReader source, BinaryWriter destination)
        {
            if (upToSourcePosition < source.BaseStream.Position) throw new ArgumentException("Can't copy backwards. Source is further than desired location");
            var distance = upToSourcePosition - source.BaseStream.Position;

            int packetSize = 10240;
            for (long i = packetSize; i < distance; i += packetSize)
            {
                var bytes = source.ReadBytes(packetSize);
                destination.Write(bytes);
            }
            var lastDistance = (int)(upToSourcePosition - source.BaseStream.Position);
            if(lastDistance > 0)
            {
                var bytes = source.ReadBytes(lastDistance);
                destination.Write(bytes);
            }

        }

        public override void Initialize(List<string> patchFiless, string targetFile)
        {
            patches.AddRange(
                patchFiless.Select(patchFileName =>
                    {
                        string noExtensionName = fs.Path.GetFileNameWithoutExtension(patchFileName);
                        string encodedParams = noExtensionName.Substring(noExtensionName.LastIndexOf('.') + 1);
                        int frameSeparationIndex = encodedParams.IndexOf("_");
                        if (frameSeparationIndex < 0)
                        {
                            return new SpritePatchFile(patchFileName, targetFile, int.Parse(encodedParams), -1);
                        }
                        else
                        {
                            string decodedId = encodedParams.Substring(0, frameSeparationIndex);
                            string decodedFrame = encodedParams.Substring(frameSeparationIndex + 2);
                            return new SpritePatchFile(patchFileName, targetFile, int.Parse(decodedId), int.Parse(decodedFrame));
                        }
                    })
                );
        }

        internal void AddPatchMark(long filePosition, int id, int frame, int width, int height)
        {
            var foundPatch = patches.Find(patch => patch.Id == id && patch.Frame == frame);
            if (foundPatch != null)
            {
                patchMarks.Add(filePosition, new PatchMark(filePosition, height, width, foundPatch));
            }
        }

    }
}

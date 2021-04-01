using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace DispelTools.GifCreator
{
    internal class ToGifConverter
    {
        private readonly List<string> filenames;
        private readonly Options options;

        private ToGifConverter(List<string> filenames, Options options)
        {
            this.filenames = filenames;
            this.options = options;
        }
        public static void ConvertToGifAndSave(List<string> filenames, Options options)
        {
            var converter = new ToGifConverter(filenames, options);
            var gif = converter.CreateGif();
            var encoder = new GifEncoder()
            {
                ColorTableMode = GifColorTableMode.Local,
                Quantizer = new OctreeQuantizer()
            };
            gif.SaveAsGif(options.OutputFilename, encoder);
        }
        private Image<Rgba32> CreateGif()
        {
            var size = CalculateMaxSize();
            var gif = new Image<Rgba32>(size.Width, size.Height);

            foreach (string filename in filenames)
            {
                using (var img = Image.Load(filename))
                {
                    var resizeOptions = new ResizeOptions()
                    {
                        Size = size,
                        Mode = ResizeMode.BoxPad,
                        PremultiplyAlpha = false,
                        Position = (AnchorPositionMode)options.Aligment,
                        Sampler = new NearestNeighborResampler()
                    };
                    img.Mutate(x => x.Resize(resizeOptions).BackgroundColor(Color.Black));
                    var frame = img.Frames[0];
                    frame.Metadata.GetGifMetadata().FrameDelay = options.Delay;
                    gif.Frames.AddFrame(frame);
                }
            }
            gif.Frames.RemoveFrame(0);
            gif.Metadata.GetGifMetadata().RepeatCount = 0;
            return gif;
        }
        private Size CalculateMaxSize()
        {
            int maxHeight = 1;
            int maxWidth = 1;
            foreach (string filename in filenames)
            {
                var info = Image.Identify(filename);
                if (info.Height > maxHeight) { maxHeight = info.Height; }
                if (info.Width > maxWidth) { maxWidth = info.Width; }
            }
            return new Size(maxWidth, maxHeight);
        }

        public class Options
        {
            private int fps;
            public string OutputFilename { get; set; }
            public enum Align { BOTTOM = AnchorPositionMode.Bottom, MIDDLE = AnchorPositionMode.Center, TOPLEFT = AnchorPositionMode.TopLeft }
            public Align Aligment { get; set; }
            public int FPS { get => fps; set { fps = value; Delay = Math.Max(fps / 100, 1); } }
            public int Delay { get; private set; }
        }
    }
}

using System.Collections.Generic;
using static DispelTools.ImageProcessing.ColorManagement;

namespace DispelTools.DataExtractor
{
    public class ExtractionParams
    {
        public enum OptionNames { ColorMode = 1, AnimatedGifs = 2, BlackAsTransparent = 4 }
        public static OptionNames NoOptions { get; } = 0;
        //Required
        public List<string> Filename { get; set; } = new();
        public string OutputDirectory { get; set; } = string.Empty;
        //Optional
        public ColorMode ColorMode { get; set; } = ColorMode.RGB16_565;
        public bool CreateAnimatedGifs { get; set; } = true;
        public bool BlackAsTransparent { get; set; } = false;
    }
}

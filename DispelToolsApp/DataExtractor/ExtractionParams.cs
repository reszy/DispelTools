using System.Collections.Generic;
using static DispelTools.ImageProcessing.ColorManagement;

namespace DispelTools.DataExtractor
{
    public class ExtractionParams
    {
        public enum OptionNames { ColorMode = 1, AnimatedGifs = 2 }
        public static OptionNames NoOptions { get; } = 0;
        //Required
        public List<string> Filename { get; set; }
        public string OutputDirectory { get; set; }
        //Optional
        public ColorMode ColorMode { get; set; } = ColorMode.RGB16_565;
        public bool CreateAnimatedGifs { get; set; } = true;
    }
}

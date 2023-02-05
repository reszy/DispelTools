using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace View.Components.PictureDisplay
{
    internal static class DisplayedImage
    {
        public static BitmapSource Create(RawBitmap imageData)
        {
            return BitmapSource.Create(imageData.Width, imageData.Height, 96, 96, PixelFormats.Rgb24, null, imageData.Bytes, imageData.Width * imageData.ByteDepth);
        }
    }
}

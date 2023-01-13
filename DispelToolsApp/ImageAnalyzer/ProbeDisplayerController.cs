using DispelTools.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispelTools.ImageAnalyzer
{
    class ProbeDisplayerController : Components.PictureDisplayer.IPictureDisplayerController
    {
        private bool showHex;
        private DispelTools.ImageAnalyzer.DataAnalyzedBitmap.DataPixel selectedPixelData;
        private Point selectedPixelCoords;

        public int RequiredTipWidth => 150;

        public void DrawHighlight(PictureDiplayer.ICoordsConverter sender, Graphics g, Point position, Pen highlightPen, float zoom)
        {
            throw new NotImplementedException();
        }

        public void DrawTip(Graphics g, Font font, Rectangle tipRectangle)
        {
            if (selectedPixelData != null)
            {
                int xPos = tipRectangle.X;
                string b;
                string w;
                string d;
                string q;
                if (showHex)
                {
                    b = selectedPixelData.Byte.ToString("X");
                    w = selectedPixelData.Word.ToString("X");
                    d = selectedPixelData.DWord.ToString("X");
                    q = selectedPixelData.QWord.ToString("X");
                }
                else
                {
                    b = selectedPixelData.Byte.ToString();
                    w = selectedPixelData.Word.ToString();
                    d = selectedPixelData.DWord.ToString();
                    q = selectedPixelData.QWord.ToString();
                }
                g.DrawString($"B: {b}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 3F));
                g.DrawString($"W: {w}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 16F));
                g.DrawString($"DW:{d}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 29F));
                g.DrawString($"QW:{q}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 42F));
            }
        }

        public void ImageReloaded(Image newImage)
        {
        }


        public void PixelSelected(PictureDiplayer.ICoordsConverter sender, PictureDiplayer.PixelSelectedArgs args)
        {
            selectedPixelCoords = args.Position;
            showHex = args.ModifierKeys == Keys.Shift;
            //selectedPixelData = DataAnalyzedBitamp?.GetPixel(selectedPixelCoords.X, selectedPixelCoords.Y);
        }
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DispelTools.Components.PictureDisplay
{
    public partial class PictureDisplayer : PictureBox
    {
        private readonly PictureDisplayerCore pictureDisplayerCore;
        private Image image;
        public enum MouseMode { Pointer, RectSelector, RowSelector };
        public MouseMode CurrentMouseMode { get; set; }

        [DefaultValue(true)]
        public bool ToolTip { get => pictureDisplayerCore.ShowDataTip; set => pictureDisplayerCore.ShowDataTip = value; }

        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.NearestNeighbor;
        public bool OffsetTileSelector { get; set; }

        public new Image Image { get => image; set => SetImage((Bitmap)value, false); }

        public delegate void PixelSelectedHandler(object sender, PixelSelectedArgs point);
        public event PixelSelectedHandler PixelSelectedEvent;

        public string DebugText { get; set; } = "";

        public PictureDisplayer()
        {
            Font = new Font(FontFamily.GenericMonospace, 10.0f);
            pictureDisplayerCore = new PictureDisplayerCore(this);

            MouseDown += pictureDisplayerCore.MouseDownAction;
            MouseUp += pictureDisplayerCore.MouseUpAction;
            MouseMove += pictureDisplayerCore.MouseMoveAction;
            MouseWheel += pictureDisplayerCore.MouseWheelAction;
            Paint += pictureDisplayerCore.PaintAction;
        }

        public void SetImage(Bitmap bitmap, bool reset, ImageAnalyzer.DataAnalyzedBitmap data = null)
        {
            image = bitmap;
            if (reset)
            {
                pictureDisplayerCore.ResetImage();
                pictureDisplayerCore.InitImageInContainer();
                pictureDisplayerCore.DataAnalyzedBitamp = data;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            paintEventArgs.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            base.OnPaint(paintEventArgs);
        }

        internal void SetController(IPictureDisplayerController displayerController)
        {
            pictureDisplayerCore.SetController(displayerController);
        }
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DispelTools.Components
{
    public partial class PictureDiplayer : PictureBox
    {
        private readonly PictureDiplayerCore pictureDiplayerCore;
        private Image image;
        public enum MouseMode { Pointer, RectSelector, RowSelector };
        public MouseMode CurrentMouseMode { get; set; }

        [DefaultValue(true)]
        public bool ToolTip { get => pictureDiplayerCore.ShowDataTip; set => pictureDiplayerCore.ShowDataTip = value; }

        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.NearestNeighbor;
        public bool OffsetTileSelector { get; set; }

        public new Image Image { get => image; set => SetImage((Bitmap)value, false); }

        public delegate void PixelSelectedHandler(object sender, PixelSelectedArgs point);
        public event PixelSelectedHandler PixelSelectedEvent;

        public string DebugText { get; set; } = "";

        public PictureDiplayer()
        {
            Font = new Font(FontFamily.GenericMonospace, 10.0f);
            pictureDiplayerCore = new PictureDiplayerCore(this);

            MouseDown += pictureDiplayerCore.MouseDownAction;
            MouseUp += pictureDiplayerCore.MouseUpAction;
            MouseMove += pictureDiplayerCore.MouseMoveAction;
            MouseWheel += pictureDiplayerCore.MouseWheelAction;
            Paint += pictureDiplayerCore.PaintAction;
        }

        public void SetImage(Bitmap bitmap, bool reset, ImageAnalyzer.DataAnalyzedBitmap data = null)
        {
            image = bitmap;
            if (reset)
            {
                pictureDiplayerCore.ResetImage();
                pictureDiplayerCore.InitImageInContainer();
                pictureDiplayerCore.DataAnalyzedBitamp = data;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            paintEventArgs.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            base.OnPaint(paintEventArgs);
        }

        internal void SetController(PictureDisplayer.IPictureDisplayerController displayerController)
        {
            pictureDiplayerCore.SetController(displayerController);
        }
    }
}

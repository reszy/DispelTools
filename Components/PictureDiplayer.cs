﻿using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DispelTools.Components
{
    public partial class PictureDiplayer : PictureBox
    {
        private PictureDiplayerCore pictureDiplayerCore;
        private Image image;

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

        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.NearestNeighbor;

        [DefaultValue(true)]
        public bool ToolTip { get => pictureDiplayerCore.ShowDataTip; set => pictureDiplayerCore.ShowDataTip = value; }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            paintEventArgs.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            base.OnPaint(paintEventArgs);
        }

        public new Image Image { get => image; set => SetImage((Bitmap)value, false); }

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
    }
}

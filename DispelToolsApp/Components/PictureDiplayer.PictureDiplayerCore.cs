using DispelTools.Viewers.MapViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static DispelTools.GameDataModels.Map.TileSet;

namespace DispelTools.Components
{
    public partial class PictureDiplayer
    {
        public interface ICoordsConverter
        {
            Point ConvertToImageCoords(Point point);
            PointF ConvertToPictureBoxCoords(Point point);
        }

        private class PictureDiplayerCore : ICoordsConverter
        {
            private readonly PictureDiplayer pictureDisplayer;

            //VIEWPORT
            private Point startingPoint;
            private Point movingPoint;
            private bool panning;

            private Point pointingAt;

            //ZOOM
            private int currentZoomStepNumber;
            private readonly double zoomStep;
            private double[] zoomStepsTable;


            //HIGLIGHT
            private Point highlight;
            private readonly Pen highlightPen;

            //DATA TIP
            private readonly Pen dataTipPen;
            private readonly Brush dataTipBrush;
            private Color selectedPixel;
            private Point selectedPixelCoords;
            private bool showHex = false;


            //SELECTION
            private bool selecting;
            private Point selectStart;
            private Point selectEnd;

            private ImageAnalyzer.DataAnalyzedBitmap.DataPixel selectedPixelData;
            private PictureDisplayer.IPictureDisplayerController subComponent;


            public bool ShowDataTip { get; internal set; } = true;
            public ImageAnalyzer.DataAnalyzedBitmap DataAnalyzedBitamp { private get; set; }
            private Bitmap Image => ((Bitmap)pictureDisplayer.Image);

            internal void SetController(PictureDisplayer.IPictureDisplayerController displayerController)
            {
                subComponent = displayerController;
            }

            public PictureDiplayerCore(PictureDiplayer pictureDisplayer)
            {
                this.pictureDisplayer = pictureDisplayer;
                zoomStep = 0.005;
                highlightPen = new Pen(Color.Black, 2);

                dataTipPen = new Pen(Color.White);
                dataTipBrush = new SolidBrush(Color.Black);

                ResetImage();
            }

            public void ResetImage()
            {
                startingPoint = Point.Empty;
                movingPoint = Point.Empty;
                panning = false;
                highlight = Point.Empty;
                currentZoomStepNumber = 0;
            }

            public Point ConvertToImageCoords(Point point)
            {
                return new Point((int)Math.Floor((point.X - movingPoint.X) / zoomStepsTable[currentZoomStepNumber]),
                                 (int)Math.Floor((point.Y - movingPoint.Y) / zoomStepsTable[currentZoomStepNumber]));
            }

            public PointF ConvertToPictureBoxCoords(Point point)
            {
                return new PointF((float)Math.Floor(point.X * zoomStepsTable[currentZoomStepNumber] + movingPoint.X),
                                 (float)Math.Floor(point.Y * zoomStepsTable[currentZoomStepNumber] + movingPoint.Y));
            }

            public void InitImageInContainer()
            {
                var imageSize = pictureDisplayer.Image.Size;
                var containerSize = pictureDisplayer.ClientRectangle.Size;
                double imageRatio = imageSize.Width / (double)imageSize.Height;
                double containerRatio = containerSize.Width / (double)containerSize.Height;
                var fitSize = containerRatio > imageRatio ?
                    new Size(imageSize.Width * containerSize.Height / imageSize.Height, containerSize.Height) :
                    new Size(containerSize.Width, imageSize.Height * containerSize.Width / imageSize.Width);

                double defaultZoom = (double)fitSize.Width / imageSize.Width;
                InitZoom(defaultZoom);
                subComponent?.ImageReloaded(pictureDisplayer.Image);
            }

            public void MouseDownAction(object sender, MouseEventArgs e)
            {
                if (Image != null)
                {
                    if (e.Button == MouseButtons.Right && !selecting)
                    {
                        Cursor.Current = Cursors.Hand;
                        panning = true;
                        startingPoint = new Point(e.Location.X, e.Location.Y);
                    }
                    else
                    {
                        subComponent?.PixelSelected(this, new PictureDiplayer.PixelSelectedArgs(highlight, Image.GetPixel(highlight.X, highlight.Y), ModifierKeys));
                        switch (pictureDisplayer.CurrentMouseMode)
                        {
                            case MouseMode.RectSelector:
                            case MouseMode.RowSelector:
                                {
                                    selectStart = selectEnd = ConvertToImageCoords(pointingAt);
                                    selecting = true;
                                }
                                break;
                            case MouseMode.Pointer:
                            default:
                                {
                                    selectedPixel = Image.GetPixel(highlight.X, highlight.Y);
                                    selectedPixelCoords = highlight;
                                    pictureDisplayer.PixelSelectedEvent?.Invoke(this, new PictureDiplayer.PixelSelectedArgs(highlight, selectedPixel, ModifierKeys));
                                }
                                break;
                        }
                    }
                }
            }

            public void MouseMoveAction(object sender, MouseEventArgs e)
            {
                if (Image != null)
                {
                    if (panning)
                    {
                        var delta = new Point(e.Location.X - startingPoint.X, e.Location.Y - startingPoint.Y);
                        startingPoint = new Point(e.Location.X, e.Location.Y);

                        movingPoint = new Point(delta.X + movingPoint.X, delta.Y + movingPoint.Y);
                    }
                    else
                    {
                        pointingAt = e.Location;
                        Highlight(ConvertToImageCoords(pointingAt));
                    }
                    if (selecting)
                    {
                        selectEnd = ConvertToImageCoords(pointingAt);
                    }
                    pictureDisplayer.Invalidate();
                }
            }

            private void InitZoom(double defaultZoomSetting)
            {
                var zoomTable = new List<double>
                {
                    defaultZoomSetting, 1, 1.25, 1.5,2, 3, 4, 5, 8, 10
                };
                if (defaultZoomSetting < 1)
                {
                    double zoom = defaultZoomSetting;
                    for (int i = 1; i < 10; zoom *= 2, i++)
                    {
                        if (1 - zoom > zoomStep)
                        {
                            zoomTable.Add(zoom);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                zoomTable.Sort();
                zoomTable = zoomTable.Distinct().ToList();

                zoomStepsTable = zoomTable.ToArray();
                currentZoomStepNumber = zoomTable.IndexOf(defaultZoomSetting);
            }

            private void Highlight(Point point)
            {
                if (point.X >= 0 && point.X < pictureDisplayer.Image.Size.Width && point.Y >= 0 && point.Y < pictureDisplayer.Image.Size.Height)
                {
                    var highlightPixelColor = Image.GetPixel(point.X, point.Y);
                    highlightPen.Color = Color.FromArgb(255 - highlightPixelColor.R, 255 - highlightPixelColor.G, 255 - highlightPixelColor.B);
                    highlight = point;
                }
            }

            private Size CalculateZoomedImageSize()
            {
                double width = pictureDisplayer.Image.Size.Width * zoomStepsTable[currentZoomStepNumber];
                double height = pictureDisplayer.Image.Size.Height * zoomStepsTable[currentZoomStepNumber];
                return new Size((int)width, (int)height);
            }

            public void PaintAction(object sender, PaintEventArgs e)
            {
                if (Image != null)
                {
                    e.Graphics.Clear(Color.Transparent);
                    e.Graphics.DrawImage(pictureDisplayer.Image, new Rectangle(movingPoint, CalculateZoomedImageSize()));
                    if (highlight != null)
                    {
                        float zoom = (float)zoomStepsTable[currentZoomStepNumber];
                        subComponent?.DrawHighlight(this, e.Graphics, highlight, highlightPen, zoom);
                        //if (pictureDisplayer.CurrentMouseMode == MouseMode.TileSelector)
                        //{
                        //    DrawTileSelector(e, zoom);
                        //}
                        //else
                        //{
                        //    var coords = ConvertToPictureBoxCoords(highlight);
                        //    e.Graphics.DrawRectangle(highlightPen, coords.X, coords.Y, zoom, zoom);
                        //}
                    }
                    if (pictureDisplayer.CurrentMouseMode != MouseMode.Pointer && selectStart != Point.Empty && selectEnd != Point.Empty)
                    {
                        DisplaySelection(e.Graphics);
                    }
                    if (ShowDataTip && (pictureDisplayer.CurrentMouseMode == MouseMode.Pointer))
                    {
                        DisplayDataTip(e.Graphics);
                    }
                    DrawDebugText(e.Graphics);
                }
            }

            private void DrawDebugText(Graphics g)
            {
                Size sizeOfText = TextRenderer.MeasureText(pictureDisplayer.DebugText, pictureDisplayer.Font);
                int startX = (int)g.ClipBounds.Width - 30 - sizeOfText.Width;
                Rectangle rect = new Rectangle(new Point(startX, 30), sizeOfText);
                g.FillRectangle(Brushes.Black, rect);
                g.DrawString(pictureDisplayer.DebugText, pictureDisplayer.Font, Brushes.White, rect);
            }

            private void DisplaySelection(Graphics g)
            {
                var start = Point.Round(ConvertToPictureBoxCoords(selectStart));
                var size = new Size(selectEnd.X - selectStart.X + 1, selectEnd.Y - selectStart.Y + 1);
                var sizeScaled = new Size((int)(size.Width * zoomStepsTable[currentZoomStepNumber]), (int)(size.Height * zoomStepsTable[currentZoomStepNumber]));
                g.DrawRectangle(highlightPen, new Rectangle(start, sizeScaled));
            }

            private void DisplayDataTip(Graphics g)
            {
                var dataTipSize = new Point(DataAnalyzedBitamp != null ? (showHex ? 400 : 350) : 250, 60);
                int yPosition = pointingAt.Y > pictureDisplayer.Size.Height / 2 ? 20 : pictureDisplayer.Size.Height - 20 - dataTipSize.Y;

                var totalTipWidth = dataTipSize.X + (subComponent?.RequiredTipWidth ?? 0);

                g.FillRectangle(dataTipBrush, 20, yPosition, totalTipWidth, dataTipSize.Y);
                g.DrawRectangle(dataTipPen, 20, yPosition, totalTipWidth, dataTipSize.Y);

                g.DrawString($"Zoom x{zoomStepsTable[currentZoomStepNumber]:0.####}", pictureDisplayer.Font, Brushes.White, new PointF(22F, yPosition + 5F));
                var pointingPosition = ConvertToImageCoords(pointingAt);
                g.DrawString($"X:{pointingPosition.X}", pictureDisplayer.Font, Brushes.White, new PointF(22F, yPosition + 18F));
                g.DrawString($"Y:{pointingPosition.Y}", pictureDisplayer.Font, Brushes.White, new PointF(22F, yPosition + 27F));

                if (selectedPixel != null)
                {
                    int xPos = 170;
                    g.DrawString($"Ch 1:{selectedPixel.R}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 3F));
                    g.DrawString($"Ch 2:{selectedPixel.G}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 16F));
                    g.DrawString($"Ch 3:{selectedPixel.B}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 29F));
                    g.DrawString($"Ch 4:{selectedPixel.A}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 42F));
                }
                subComponent?.DrawTip(g, pictureDisplayer.Font, new Rectangle(250, yPosition, dataTipSize.X, dataTipSize.Y));
            }

            public void MouseUpAction(object sender, MouseEventArgs e)
            {
                Cursor.Current = Cursors.Default;
                panning = false;
                selecting = false;
            }

            public void MouseWheelAction(object sender, MouseEventArgs e)
            {
                if (Image != null)
                {
                    if (zoomStepsTable != null && !panning)
                    {
                        var oldposition = ConvertToImageCoords(Point.Empty);
                        currentZoomStepNumber += e.Delta / SystemInformation.MouseWheelScrollDelta;
                        if (currentZoomStepNumber >= zoomStepsTable.Length)
                        {
                            currentZoomStepNumber = zoomStepsTable.Length - 1;
                        }
                        else if (currentZoomStepNumber < 0)
                        {
                            currentZoomStepNumber = 0;
                        }
                        else
                        {
                            movingPoint = Point.Empty;
                            var newPosition = ConvertToPictureBoxCoords(oldposition);
                            movingPoint = new Point(-(int)newPosition.X, -(int)newPosition.Y);
                        }
                        pictureDisplayer.Invalidate();
                    }
                }
            }
        }
    }
}

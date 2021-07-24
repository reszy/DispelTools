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
        private class PictureDiplayerCore
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


            public bool ShowDataTip { get; internal set; } = true;
            public ImageAnalyzer.DataAnalyzedBitmap DataAnalyzedBitamp { private get; set; }
            private Bitmap Image => ((Bitmap)pictureDisplayer.Image);

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
                            case MouseMode.TileSelector:
                            default:
                            {
                                selectedPixel = Image.GetPixel(highlight.X, highlight.Y);
                                selectedPixelData = DataAnalyzedBitamp?.GetPixel(highlight.X, highlight.Y);
                                showHex = ModifierKeys == Keys.Shift;
                                selectedPixelCoords = highlight;
                                pictureDisplayer.PixelSelectedEvent?.Invoke(this, new PictureDiplayer.PixelSelectedArgs(selectedPixelCoords, selectedPixel));
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
                        if (pictureDisplayer.CurrentMouseMode == MouseMode.TileSelector)
                        {
                            DrawTileSelector(e, zoom);
                        }
                        else
                        {
                            var coords = ConvertToPictureBoxCoords(highlight);
                            e.Graphics.DrawRectangle(highlightPen, coords.X, coords.Y, zoom, zoom);
                        }
                    }
                    if (pictureDisplayer.CurrentMouseMode != MouseMode.Pointer && selectStart != Point.Empty && selectEnd != Point.Empty)
                    {
                        DisplaySelection(e.Graphics);
                    }
                    if (ShowDataTip && (pictureDisplayer.CurrentMouseMode == MouseMode.Pointer || pictureDisplayer.CurrentMouseMode == MouseMode.TileSelector))
                    {
                        DisplayDataTip(e.Graphics);
                    }
                }
            }

            private void DrawTileSelector(PaintEventArgs e, float zoom)
            {
                var tileCoords = ConvertToPictureBoxCoords(GetClosestTileCenter(highlight));
                var points = new PointF[]
                {
                    new PointF(tileCoords.X - (32*zoom), tileCoords.Y),
                    new PointF(tileCoords.X, tileCoords.Y - (16*zoom)),
                    new PointF(tileCoords.X + (32*zoom), tileCoords.Y),
                    new PointF(tileCoords.X, tileCoords.Y + (16*zoom)),
                    new PointF(tileCoords.X - (32*zoom), tileCoords.Y),

                };
                e.Graphics.DrawLines(highlightPen, points);
            }

            private bool IsCenterOfTile(Point point) => ((point.X & 1) == (point.Y & 1)) ^ pictureDisplayer.OffsetTileSelector ;

            private Point GetClosestTileCenter(Point pointerCoords)
            {
                double yTileDistance = pointerCoords.Y / (double)TILE_HEIGHT_HALF;
                double xTileDistance = pointerCoords.X / (double)TILE_HORIZONTAL_OFFSET_HALF;
                int top = (int)Math.Floor(yTileDistance);
                int bottom = (int)Math.Ceiling(yTileDistance);
                int left = (int)Math.Floor(xTileDistance);
                int right = (int)Math.Ceiling(xTileDistance);

                var corners = new Point[]
                {
                    new Point(left, top),
                    new Point(right, top),
                    new Point(left, bottom),
                    new Point(right, bottom)
                };
                var possibleCoords = new HashSet<Point>();
                foreach (var corner in corners)
                {
                    if (IsCenterOfTile(corner))
                    {
                        possibleCoords.Add(new Point(corner.X * TILE_HORIZONTAL_OFFSET_HALF, corner.Y * TILE_HEIGHT_HALF));
                    }
                }
                var bestCoords = Point.Empty;
                double? bestDistance = null;
                foreach (var coords in possibleCoords)
                {
                    float xPow = (coords.X - pointerCoords.X) * (coords.X - pointerCoords.X);
                    float yPow = (coords.Y - pointerCoords.Y) * (coords.Y - pointerCoords.Y);
                    double distance = Math.Sqrt(xPow + yPow);
                    if (bestDistance == null || distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestCoords = coords;
                    }
                }
                return bestCoords;
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

                g.FillRectangle(dataTipBrush, 20, yPosition, dataTipSize.X, dataTipSize.Y);
                g.DrawRectangle(dataTipPen, 20, yPosition, dataTipSize.X, dataTipSize.Y);

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
                if (selectedPixelData != null)
                {
                    int xPos = 250;
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
                    g.DrawString($"B: {b}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 3F));
                    g.DrawString($"W: {w}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 16F));
                    g.DrawString($"DW:{d}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 29F));
                    g.DrawString($"QW:{q}", pictureDisplayer.Font, Brushes.White, new PointF(xPos, yPosition + 42F));
                }
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

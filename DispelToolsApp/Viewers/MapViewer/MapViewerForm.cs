﻿using DispelTools.Common;
using DispelTools.Components;
using DispelTools.GameDataModels.Map;
using DispelTools.GameDataModels.Map.Generator;
using DispelTools.GameDataModels.Map.Reader;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DispelTools.Viewers.MapViewer
{
    public partial class MapViewerForm : Form
    {
        private readonly BackgroundWorker backgroundWorker;


        private DirectBitmap image;
        private DirectBitmap tileImage;

        private string filename;
        private bool generatedOccluded;

        private MapContainer mapContainer;
        private bool MapLoaded => mapContainer != null;

        public MapViewerForm()
        {
            InitializeComponent();

            pictureBox1.PixelSelectedEvent += TileClicked;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += LoadMap;
            backgroundWorker.ProgressChanged += ProgressChanged;
            backgroundWorker.RunWorkerCompleted += LoadingCompleted;

            toolTip.SetToolTip(occludeCheckBox, "Prevents from generating corners of map that are not visible in game");
            toolTip.SetToolTip(gtlCheckBox, "Shows ground tiles");
            toolTip.SetToolTip(btlCheckBox, "Shows tiles that can overlap other tiles");
            toolTip.SetToolTip(collisionsCheckBox, "Shows which tiles can be accessed by player");
            toolTip.SetToolTip(roofsCheckBox, "Shows which tiles are signed as bldg");
            toolTip.SetToolTip(spritesCheckBox, "Shows sprites which are included in map file");
        }

        private void TileClicked(object sender, PictureDiplayer.PixelSelectedArgs point)
        {
            //var mapPosition = mapReader.TranslateImageToMapPosition(point.Position);
            //Clipboard.SetText($"TELEPORT({mapPosition.X},{mapPosition.Y})");
        }

        private void LoadingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tileSetCombo_SelectedIndexChanged(null, EventArgs.Empty);
            var sb = new StringBuilder();
            sb.Append(mapContainer.GetStats());
            if (image != null)
            {
                pictureBox1.SetImage(image.Bitmap, true);
                pictureBox1.OffsetTileSelector = generatedOccluded ^ (image.Height / TileSet.TILE_HEIGHT % 2 == 0);

                sb.AppendLine();
                sb.AppendLine("--Image--");
                sb.AppendLine($"Image size: {image.Width}x{image.Height}");
                sb.Append($"Memory size: {BytesFormatter.GetBytesReadable(image.Bits.Length * 4)}");
            }
            statsTextBox.Text = sb.ToString();
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (e.UserState is string text)
            {
                if (text != progressBar.Text)
                {
                    progressBar.Text = text;
                }
            }
        }

        private void LoadMap(object sender, DoWorkEventArgs e)
        {
            var workReporter = new WorkReporter(backgroundWorker, 4);
            var mapReader = new MapReader(filename, workReporter);
            if (mapContainer == null)
            {
                workReporter.StartNewStage(1, "Loading Map...");
                mapContainer = mapReader.ReadMap(false);

                workReporter.StartNewStage(2, "Loading GTL...");
                mapContainer.Gtl = TileSet.LoadTileSet(filename.Replace(".map", ".gtl"), workReporter);

                workReporter.StartNewStage(3, "Loading BTL...");
                mapContainer.Btl = TileSet.LoadTileSet(filename.Replace(".map", ".btl"), workReporter);
            }

            workReporter.StartNewStage(4, "Generating map...");
            var mapGenerator = new MapImageGenerator(workReporter, mapContainer);
            image = mapGenerator.GenerateMap(
                new GeneratorOptions()
                {
                    Occlusion = generatedOccluded,
                    GTL = gtlCheckBox.Checked,
                    Collisions = collisionsCheckBox.Checked,
                    TiledObjects = btlCheckBox.Checked,
                    Roofs = roofsCheckBox.Checked,
                    Sprites = spritesCheckBox.Checked
                });
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(() =>
            {
                filename = openFileDialog.FileName;
                mapContainer?.Dispose();
                mapContainer = null;
            });
        }

        private void tileShowNumber_ValueChanged(object sender, EventArgs e)
        {
            if (MapLoaded)
            {
                if (tileSetCombo.SelectedIndex == 0)
                {
                    ShowTile(mapContainer.Gtl[(int)tileShowNumber.Value]);
                }

                if (tileSetCombo.SelectedIndex == 1)
                {
                    ShowTile(mapContainer.Btl[(int)tileShowNumber.Value]);
                }

                if (tileSetCombo.SelectedIndex == 2)
                {
                    tileImage?.Dispose();
                    var bmp = mapContainer.InternalSprites[(int)tileShowNumber.Value].GetFrame(0).Bitmap;
                    tileDiplayer.SetImage(bmp.Bitmap, true);
                }
            }
        }

        private void ShowTile(TileSet.Tile tile)
        {
            tileImage?.Dispose();
            tileImage = new DirectBitmap(TileSet.TILE_WIDTH, TileSet.TILE_HEIGHT);
            tile.PlotTileOnBitmap(ref tileImage, 0, 0);
            tileDiplayer.SetImage(tileImage.Bitmap, true);
        }

        private void tileSetCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapLoaded && mapContainer.Gtl != null && mapContainer.Btl != null)
            {
                if (tileSetCombo.SelectedIndex == 0)
                {
                    tileShowNumber.Maximum = mapContainer.Gtl.Count - 1;
                }
                if (tileSetCombo.SelectedIndex == 1)
                {
                    tileShowNumber.Maximum = mapContainer.Btl.Count - 1;
                }
                if (tileSetCombo.SelectedIndex == 2)
                {
                    tileShowNumber.Maximum = mapContainer.InternalSprites.Count - 1;
                }
                tileShowNumber.Value = Math.Min(tileShowNumber.Value, tileShowNumber.Maximum);
                tileShowNumber_ValueChanged(null, EventArgs.Empty);
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(filename) && !backgroundWorker.IsBusy)
            {
                if (mapContainer == null)
                {
                    tileShowNumber.Value = 0;
                    tileShowNumber.Maximum = 0;
                    progressBar.Maximum = 4000;
                }
                image?.Dispose();
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = null;
                generatedOccluded = occludeCheckBox.Checked;
                backgroundWorker.RunWorkerAsync();
            }
        }

        // Debug function
        private void button1_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Settings.GameRootDir + @"\Map", "*.map");
            var worker = new BackgroundWorker();
            foreach (string file in files)
            {
                var reader = new MapReader(file, new WorkReporter(worker));
                var map = reader.ReadMap(true);
                map.Dispose();
            }
            worker.Dispose();
        }
    }
}

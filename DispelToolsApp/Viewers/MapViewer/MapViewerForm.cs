using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace DispelTools.Viewers.MapViewer
{
    public partial class MapViewerForm : Form
    {
        private readonly BackgroundWorker backgroundWorker;
        private MapReader mapReader;
        private DirectBitmap image;

        private static readonly Dictionary<string, int> knownMapCoords = new Dictionary<string, int>()
        {
            { "map1", 5996704 },//11835
            { "map2", 6162088 },//16848
            { "map3", 5733690 },//13121
            { "cat1", 408914 }//3614//3783
        };

        public MapViewerForm()
        {
            InitializeComponent();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += LoadMap;
            backgroundWorker.ProgressChanged += ProgressChanged;
            backgroundWorker.RunWorkerCompleted += LoadingCompleted;
        }

        private void LoadingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tileSetCombo_SelectedIndexChanged(null, EventArgs.Empty);
            if (image != null)
            {
                pictureBox1.SetImage(image.Bitmap, true);
            }
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
            tileShowNumber.Value = 0;
            tileShowNumber.Maximum = 0;
            progressBar.Maximum = 3000;
            image?.Dispose();
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
            image = mapReader.GenerateMap();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(() =>
            {
                string filename = openFileDialog.FileName;
                if (knownMapCoords.TryGetValue(Path.GetFileNameWithoutExtension(filename), out var offset))
                {
                    mapReader = new MapReader(filename, offset, backgroundWorker);
                }
                else
                {
                    mapReader = new MapReader(filename);
                }
            });
        }

        private void tileShowNumber_ValueChanged(object sender, EventArgs e)
        {
            if (mapReader.Gtl != null && mapReader.Btl != null)
            {
                if (tileSetCombo.SelectedIndex == 0)
                {
                    ShowTile(mapReader.Gtl[(int)tileShowNumber.Value]);
                }

                if (tileSetCombo.SelectedIndex == 1)
                {
                    ShowTile(mapReader.Btl[(int)tileShowNumber.Value]);
                }
            }
        }

        private void ShowTile(TileSet.Tile tile)
        {
            var bmp = new DirectBitmap(TileSet.TILE_WIDTH, TileSet.TILE_HEIGHT);
            tile.PlotTileOnBitmap(ref bmp, 0, 0);
            tileDiplayer.Image?.Dispose();
            tileDiplayer.SetImage(bmp.Bitmap, true);
        }

        private void tileSetCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mapReader != null && mapReader.Gtl != null && mapReader.Btl != null)
            {
                if (tileSetCombo.SelectedIndex == 0)
                {
                    tileShowNumber.Maximum = mapReader.Gtl.Count - 1;
                }
                if (tileSetCombo.SelectedIndex == 1)
                {
                    tileShowNumber.Maximum = mapReader.Btl.Count - 1;
                }
                tileShowNumber.Value = Math.Min(tileShowNumber.Value, tileShowNumber.Maximum);
                tileShowNumber_ValueChanged(null, EventArgs.Empty);
            }
        }

        private void gtlCheckBox_CheckedChanged(object sender, EventArgs e) => mapReader.GTL = gtlCheckBox.Checked;

        private void collisionsCheckBox_CheckedChanged(object sender, EventArgs e) => mapReader.Collisions = collisionsCheckBox.Checked;

        private void btlCheckBox_CheckedChanged(object sender, EventArgs e) => mapReader.BTL = btlCheckBox.Checked;

        private void bldgCheckBox_CheckedChanged(object sender, EventArgs e) => mapReader.BLDG = bldgCheckBox.Checked;

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (mapReader != null)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mapReader != null)
            {
                    mapReader.SeekMapTiles();
            }
        }
    }
}

using DispelTools.Common;
using static DispelTools.Common.MagickImageSave;
using DispelTools.Common.DataProcessing;
using DispelTools.Components.PictureDisplay;
using DispelTools.DebugTools.MetricTools;
using DispelTools.GameDataModels.Map;
using DispelTools.GameDataModels.Map.Generator;
using DispelTools.GameDataModels.Map.Reader;
using DispelTools.ImageProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DispelTools.GameDataModels.Map.External.Extra;
using DispelTools.GameDataModels.Map.External.Monster;
using DispelTools.GameDataModels.Map.External.Npc;

namespace DispelTools.Viewers.MapViewer
{
    public partial class MapViewerForm : Form
    {
        private readonly BackgroundWorker backgroundWorker;


        private DirectBitmap mapImage;
        private DirectBitmap sidePreviewImage;
        private readonly TextGenerator textGenerator;

        private MapDisplayerController mapDisplayerController;

        private string filename;
        private bool generatedOccluded;

        private MapContainer mapContainer;
        private bool MapLoaded => mapContainer != null;

        public MapViewerForm()
        {
            InitializeComponent();
            textGenerator = new TextGenerator(new Font(FontFamily.GenericMonospace, 8.0f));

            pictureBox.PixelSelectedEvent += TileClicked;

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
#if DEBUG
            debugButton.Visible = debugDotsCheckBox.Visible = true;
#else
            debugButton.Visible = debugDotsCheckBox.Visible = false;
#endif

            var setting = Settings.MapGenerationOptions;
            occludeCheckBox.Checked = generatedOccluded = setting.Occlusion;
            gtlCheckBox.Checked = setting.GTL;
            collisionsCheckBox.Checked = setting.Collisions;
            btlCheckBox.Checked = setting.TiledObjects;
            roofsCheckBox.Checked = setting.Roofs;
            spritesCheckBox.Checked = setting.Sprites;
            eventsCheckBox.Checked = setting.Events;
            extraCheckBox.Checked = setting.ExternalExtra;
            monsterCheckBox.Checked = setting.ExternalMonster;
            npcCheckBox.Checked = setting.ExternalNpc;
            debugDotsCheckBox.Checked = setting.DebugDots;

            toggleClipBoard.Checked = Settings.CopyCommandToClipboard;

            mapDisplayerController = new MapDisplayerController();
            mapDisplayerController.InfoRequestedEvent += CreateTileInfo;
            pictureBox.SetController(mapDisplayerController);
        }

        private void CreateTileInfo(Point point, List<MapDisplayerController.TileInfo> info)
        {
            if (MapLoaded)
            {
                var mapPosition = mapContainer.TranslateImageToMapCoords(point.X, point.Y, generatedOccluded);

                info.AddRange(SearchForTile(mapContainer.ExtraEntities, mapPosition, "Extra"));
                info.AddRange(SearchForTile(mapContainer.MonsterEntities, mapPosition, "Monster"));
                info.AddRange(SearchForTile(mapContainer.NpcEntities, mapPosition, "Npc"));
            }
        }

        private IEnumerable<MapDisplayerController.TileInfo> SearchForTile(List<GameDataModels.Map.External.MapExternalObject> objects, Point mapPosition, string type)
        {
            return objects
                .Where(e => e.X == mapPosition.X && e.Y == mapPosition.Y)
                .Select(e => new MapDisplayerController.TileInfo(type, e.DbId, e.SpriteName, e.SpriteId));
        }

        private void TileClicked(object sender, PixelSelectedArgs point)
        {
            if (MapLoaded)
            {
                var mapPosition = mapContainer.TranslateImageToMapCoords(point.Position.X, point.Position.Y, generatedOccluded);
                pictureBox.DebugText = $"Tile({mapPosition.X}, {mapPosition.Y})";
                if (toggleClipBoard.Checked) Clipboard.SetText($"TELEPORT({mapPosition.X},{mapPosition.Y})");
            }
        }

        private void LoadingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tileSetCombo_SelectedIndexChanged(null, EventArgs.Empty);
            var sb = new StringBuilder();
            sb.Append(mapContainer.GetStats());
            if (mapImage != null)
            {
                pictureBox.SetImage(mapImage.Bitmap, true);
                pictureBox.OffsetTileSelector = generatedOccluded ^ (mapImage.Height / TileSet.TILE_HEIGHT % 2 == 0);

                sb.AppendLine();
                sb.AppendLine("--Image--");
                sb.AppendLine($"Image size: {mapImage.Width}x{mapImage.Height}");
                sb.Append($"Memory size: {BytesFormatter.GetBytesReadable(mapImage.Bits.Length * 4)}");
            }
            progressBar.Text = "Completed";
            statsTextBox.Text = sb.ToString();
            saveAsImageButton.Enabled = true;
            mapDisplayerController.EvenTiles = mapContainer.Model.TiledMapSize.Height % 2 == 0;
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
            var workReporter = new WorkReporter(backgroundWorker, 5);
            progressBar.Maximum = workReporter.StagesLeft * 1000;
            var mapReader = new MapReader(filename, workReporter);
            if (mapContainer == null)
            {
                workReporter.StartNewStage(1, "Loading Map...");
                mapContainer = mapReader.ReadMap(false);

                workReporter.StartNewStage(2, "Loading GTL...");
                mapContainer.Gtl = TileSet.LoadTileSet(filename.Replace(".map", ".gtl"), workReporter);

                workReporter.StartNewStage(3, "Loading BTL...");
                mapContainer.Btl = TileSet.LoadTileSet(filename.Replace(".map", ".btl"), workReporter);

                workReporter.StartNewStage(4, "Loading external extra, monster, npc...");
                LoadExternal(workReporter);
            }

            workReporter.StartNewStage(5, "Generating map...");
            var mapGenerator = new MapImageGenerator(workReporter, mapContainer, textGenerator);

            mapImage = mapGenerator.GenerateMap(CheckBoxesToGenerationOptions());
        }

        private void LoadExternal(WorkReporter workReporter)
        {
            workReporter.SetTotal(4);
            AddEntities(mapContainer.ExtraEntities, new MapExtraReader());
            workReporter.ReportProgress(1);
            AddEntities(mapContainer.MonsterEntities, new MapMonsterReader());
            AddEntities(mapContainer.MonsterEntities, new MultiplayerMapMonsterReader());
            workReporter.ReportProgress(2);
            AddEntities(mapContainer.NpcEntities, new MapNpcReader());
            workReporter.ReportProgress(3);
        }

        private void AddEntities(List<GameDataModels.Map.External.MapExternalObject> target, GameDataModels.Map.External.IExternalEntitiesReader reader)
        {
            try
            {
                target.AddRange(reader.GetObjects(Settings.GameRootDir, filename, mapContainer));
            }
            catch (ReadFileException e)
            {
                MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private GeneratorOptions CheckBoxesToGenerationOptions()
        {
            return new GeneratorOptions
               (
                   generatedOccluded,
                   gtlCheckBox.Checked,
                   spritesCheckBox.Checked,
                   collisionsCheckBox.Checked,
                   btlCheckBox.Checked,
                   roofsCheckBox.Checked,
                   eventsCheckBox.Checked,
                   extraCheckBox.Checked,
                   monsterCheckBox.Checked,
                   npcCheckBox.Checked,
                   debugDotsCheckBox.Checked
               );
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(() =>
            {
                filename = openFileDialog.FileName;
                saveImageDialog.InitialDirectory = Path.GetDirectoryName(filename);
                saveImageDialog.FileName = Path.GetFileNameWithoutExtension(filename);
                mapContainer?.Dispose();
                mapContainer = null;
                selectedMapLabel.Text = Path.GetFileName(filename);
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
                    sidePreviewImage?.Dispose();
                    sidePreviewImage = mapContainer.InternalSprites[(int)tileShowNumber.Value].GetFrame(0).RawRgb.ToDirectBitmap();
                    tileDisplayer.SetImage(sidePreviewImage.Bitmap, true);
                }
            }
        }

        private void ShowTile(TileSet.Tile tile)
        {
            sidePreviewImage?.Dispose();
            sidePreviewImage = new DirectBitmap(TileSet.TILE_WIDTH, TileSet.TILE_HEIGHT);
            tile.PlotTileOnBitmap(sidePreviewImage, 0, 0);
            tileDisplayer.SetImage(sidePreviewImage.Bitmap, true);
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
                }

                saveAsImageButton.Enabled = false;

                mapImage?.Dispose();
                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
                generatedOccluded = occludeCheckBox.Checked;
                backgroundWorker.RunWorkerAsync();
            }
        }

        // Debug function
        private void debugButton_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Settings.GameRootDir + @"\Map\Multi", "*.mon");
            using (var worker = new BackgroundWorker())
            {
                var results = new Dictionary<string, List<(int id, int count)>>();
                foreach (string file in files)
                {
                    var fileFingings = new List<int>();
                    var lines = File.ReadAllLines(file);
                    foreach (var line in lines)
                    {
                        if (!line.StartsWith(";"))
                        {
                            var id = int.Parse(line.Split(',')[1]);
                            if (id > 36) fileFingings.Add(id);
                        }
                    }
                    if(fileFingings.Count>0)
                    results[Path.GetFileNameWithoutExtension(file)] = fileFingings.GroupBy(x => x)
                        .Select(g => (g.Key, g.Count())).ToList();
                }
                var json = string.Join(",", results.Select(entry => $"\"{entry.Key}\" : [{string.Join(",", entry.Value.Select(g => $"{{ \"id\":{g.id}, \"count\":{g.count}}}").ToArray())}]").ToArray());
            }
        }

        private void saveAsImageButton_Click(object sender, EventArgs e)
        {
            saveImageDialog.Filter = Filter;
            var result = saveImageDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(saveImageDialog.FileName))
            {
                mapImage.SaveAs(saveImageDialog.FileName, Encoders[saveImageDialog.FilterIndex - 1]);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (mapContainer != null) Settings.MapGenerationOptions = CheckBoxesToGenerationOptions();
            Settings.CopyCommandToClipboard = toggleClipBoard.Checked;
            base.OnClosed(e);
        }

        private void toggleClipBoard_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

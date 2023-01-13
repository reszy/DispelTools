using DispelTools.Common;
using static DispelTools.Common.MagickImageSave;
using DispelTools.Common.DataProcessing;
using DispelTools.Components;
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

            mapDisplayerController = new MapDisplayerController();
            mapDisplayerController.InfoRequestedEvent += CreateTileInfo;
            pictureBox.SetController(mapDisplayerController);
        }

        private void CreateTileInfo(Point point, List<MapDisplayerController.TileInfo> info)
        {
            if(MapLoaded)
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
                .Select(e => new MapDisplayerController.TileInfo(type, e.DbId, e.SpriteName));
        }

        private void TileClicked(object sender, PictureDiplayer.PixelSelectedArgs point)
        {
            if (MapLoaded)
            {
                var mapPosition = mapContainer.TranslateImageToMapCoords(point.Position.X, point.Position.Y, generatedOccluded);
                pictureBox.DebugText = $"Tile({mapPosition.X}, {mapPosition.Y})";
                Clipboard.SetText($"TELEPORT({mapPosition.X},{mapPosition.Y})");
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
            var mapName = Path.GetFileNameWithoutExtension(filename);
            workReporter.SetTotal(4);
            mapContainer.ExtraEntities.AddRange(new MapExtraReader().GetObjects(Settings.GameRootDir, mapName, mapContainer));
            workReporter.ReportProgress(1);
            mapContainer.MonsterEntities.AddRange(new MapMonsterReader().GetObjects(Settings.GameRootDir, mapName, mapContainer));
            workReporter.ReportProgress(2);
            mapContainer.NpcEntities.AddRange(new MapNpcReader().GetObjects(Settings.GameRootDir, mapName, mapContainer));
            workReporter.ReportProgress(3);
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
                    tileDiplayer.SetImage(sidePreviewImage.Bitmap, true);
                }
            }
        }

        private void ShowTile(TileSet.Tile tile)
        {
            sidePreviewImage?.Dispose();
            sidePreviewImage = new DirectBitmap(TileSet.TILE_WIDTH, TileSet.TILE_HEIGHT);
            tile.PlotTileOnBitmap(sidePreviewImage, 0, 0);
            tileDiplayer.SetImage(sidePreviewImage.Bitmap, true);
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
            string[] files = Directory.GetFiles(Settings.GameRootDir + @"\Map", "*.map");
            using (var worker = new BackgroundWorker())
            {
                var doubled = new Dictionary<string, List<GameDataModels.Map.External.MapExternalObject>>();
                foreach (string file in files)
                {
                    if (file.Contains("map4.map")) continue;
                    var reader = new MapReader(file, new WorkReporter(worker));
                    using (var map = reader.ReadMap(true))
                    {
                        map.ExtraEntities.AddRange(new MapExtraReader().GetObjects(Settings.GameRootDir, map.MapName, map));
                        map.MonsterEntities.AddRange(new MapMonsterReader().GetObjects(Settings.GameRootDir, map.MapName, map));
                        map.NpcEntities.AddRange(new MapNpcReader().GetObjects(Settings.GameRootDir, map.MapName, map));

                        var all = new List<GameDataModels.Map.External.MapExternalObject>(map.MonsterEntities);
                        all.AddRange(map.ExtraEntities);
                        all.AddRange(map.NpcEntities);

                        doubled[map.MapName] = all.GroupBy(entity => new Point(entity.X, entity.Y))
                                       .Where(grouped => grouped.Count() > 1)
                                       .SelectMany(group => group.ToList())
                                       .ToList();
                    }
                }
                var maps = doubled.Where(entry => entry.Value.Count > 0)
                    .Select(entry =>
                    {
                        var objects = entry.Value.Select(x => $"{{\"X\": {x.X}, \"Y\": {x.Y}}}").ToArray();
                        var listElements = string.Join(", ", objects);
                        return $"\"{entry.Key}\": {{ \"duplicates\": [{listElements}] }}";
                    }
                    ).ToArray();
                var mapsStr = $"{{{string.Join(", ", maps)}}}";
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
            base.OnClosed(e);
        }
    }
}

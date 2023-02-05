using DispelTools.Common;
using static DispelTools.Common.MagickImageSave;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileDialogs;
using DispelTools.GameDataModels.Map.Generator;
using DispelTools.GameDataModels.Map;
using System.ComponentModel;
using DispelTools.Common.DataProcessing;
using DispelTools.GameDataModels.Map.Reader;
using DispelTools.ImageProcessing;
using View.Components.PictureDisplay;
using View.ViewModels;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for MapViewerView.xaml
    /// </summary>
    public partial class MapViewerView : UserControl, INestedView
    {
        private readonly FileSystem fs;
        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveImageDialog;

        private readonly BackgroundWorker backgroundWorker;

        private RawBitmap? mapImage;
        private RawBitmap? sidePreviewImage;
        private readonly TextGenerator textGenerator;

        private readonly MapViewerController mapViewerController;

        private string filename;
        private GeneratorOptions checkboxOptions;
        private bool generatedOccluded;

        private MapContainer? mapContainer;
        public MapViewerView()
        {
            InitializeComponent();
            fs = new FileSystem();
            filename = string.Empty;

            //textGenerator = new TextGenerator(new Font(FontFamily.GenericMonospace, 8.0f));
            mapViewerController = new(this);
            mapDisplay.SetController(mapViewerController);

            TileShowNumber.ValueChanged += TileShowNumberChanged;

            backgroundWorker = new();
            backgroundWorker.DoWork += LoadMap;
            backgroundWorker.ProgressChanged += ProgressChanged;
            backgroundWorker.RunWorkerCompleted += LoadingCompleted;

            var setting = checkboxOptions = Settings.MapGenerationOptions;
            OccludeCheckBox.IsChecked = generatedOccluded = setting.Occlusion;
            GtlCheckBox.IsChecked = setting.GTL;
            CollisionsCheckBox.IsChecked = setting.Collisions;
            BtlCheckBox.IsChecked = setting.TiledObjects;
            RoofsCheckBox.IsChecked = setting.Roofs;
            SpritesCheckBox.IsChecked = setting.Sprites;
            EventsCheckBox.IsChecked = setting.Events;
            ExtraCheckBox.IsChecked = setting.ExternalExtra;
            MonsterCheckBox.IsChecked = setting.ExternalMonster;
            NpcCheckBox.IsChecked = setting.ExternalNpc;
            DebugDotsCheckBox.IsChecked = setting.DebugDots;

            ToggleClipBoard.IsChecked = Settings.CopyCommandToClipboard;

            openFileDialog = new(fs, Window.GetWindow(this), new OpenFileDialog.Configuration()
            {
                Filter = "Dispel map|*.map"
            });

            saveImageDialog = new(fs, Window.GetWindow(this), new SaveFileDialog.Configuration()
            {
                Filter = Filter
            });
        }

        public string ViewName => "Map Viewer";

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            openFileDialog.ShowDialog(() =>
            {
                filename = openFileDialog.FileName;
                //saveImageDialog.InitialDirectory = fs.Path.GetDirectoryName(filename);
                //saveImageDialog.FileName = fs.Path.GetFileNameWithoutExtension(filename);
                mapContainer?.Dispose();
                mapContainer = null;
                SelectedMapLabel.Text = fs.Path.GetFileName(filename);
            });
        }
        private void GenerateClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(filename) && !backgroundWorker.IsBusy)
            {
                if (mapContainer == null)
                {
                    TileShowNumber.Value = 0;
                    TileShowNumber.Maximum = 0;
                }

                SaveAsImageButton.IsEnabled = false;

                mapDisplay.ClearImage();
                checkboxOptions = CheckBoxesToGenerationOptions();
                generatedOccluded = OccludeCheckBox.IsChecked ?? false;
                backgroundWorker.RunWorkerAsync();
            }
        }
        private void SaveAsImageClick(object sender, RoutedEventArgs e)
        {
            saveImageDialog.ShowDialog(() =>
            {
                if (!string.IsNullOrEmpty(saveImageDialog.FileName))
                {
                    mapImage?.SaveAs(saveImageDialog.FileName, Encoders[saveImageDialog.FilterIndex - 1]);
                }
            });
        }

        private void TileShowTypeChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (mapContainer is not null && mapContainer.Gtl != null && mapContainer.Btl != null)
            {
                if (TileSetCombo.SelectedIndex == 0)
                {
                    TileShowNumber.Maximum = mapContainer.Gtl.Count - 1;
                }
                if (TileSetCombo.SelectedIndex == 1)
                {
                    TileShowNumber.Maximum = mapContainer.Btl.Count - 1;
                }
                if (TileSetCombo.SelectedIndex == 2)
                {
                    TileShowNumber.Maximum = mapContainer.InternalSprites.Count - 1;
                }
                TileShowNumber.Value = Math.Min(TileShowNumber.Value, TileShowNumber.Maximum);
                TileShowNumberChanged(null, new RoutedEventArgs());
            }
        }

        private void TileShowNumberChanged(object? sender, RoutedEventArgs e)
        {
            if (mapContainer is not null)
            {
                if (TileSetCombo.SelectedIndex == 0)
                {
                    ShowTile(mapContainer.Gtl![(int)TileShowNumber.Value]);
                }

                if (TileSetCombo.SelectedIndex == 1)
                {
                    ShowTile(mapContainer.Btl![(int)TileShowNumber.Value]);
                }

                if (TileSetCombo.SelectedIndex == 2)
                {
                    sidePreviewImage = mapContainer.InternalSprites[(int)TileShowNumber.Value].GetFrame(0).RawRgb;
                    //TileDisplayer.SetImage(sidePreviewImage.Bitmap, true);
                }
            }
        }

        internal void CreateTileInfo(System.Drawing.Point point)
        {
            if (mapContainer is not null)
            {
                var mapPosition = mapContainer.TranslateImageToMapCoords(point.X, point.Y, generatedOccluded);

                var infos = new List<MapTileInfo>();
                infos.AddRange(SearchForTile(mapContainer.ExtraEntities, mapPosition, "Extra"));
                infos.AddRange(SearchForTile(mapContainer.MonsterEntities, mapPosition, "Monster"));
                infos.AddRange(SearchForTile(mapContainer.NpcEntities, mapPosition, "Npc"));

                var additionalInfo = new AdditionalInfo("", "ID:", "SPR:", "SprId");
                foreach (var info in infos)
                {
                    additionalInfo.Add(info.Type, info.Id.ToString(), info.SpriteName, info.SpriteId.ToString());
                }
                mapDisplay.DrawAdditionalInfo(additionalInfo);

                mapDisplay.DrawAdditionalText($"Tile({mapPosition.X}, {mapPosition.Y})");
                if (ToggleClipBoard.IsChecked ?? false) Clipboard.SetText($"TELEPORT({mapPosition.X},{mapPosition.Y})");
            }
        }

        private IEnumerable<MapTileInfo> SearchForTile(List<DispelTools.GameDataModels.Map.External.MapExternalObject> objects, System.Drawing.Point mapPosition, string type)
        {
            return objects
                .Where(e => e.X == mapPosition.X && e.Y == mapPosition.Y)
                .Select(e => new MapTileInfo(type, e.DbId, e.SpriteName, e.SpriteId));
        }

        private void LoadingCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            TileShowNumberChanged(null, new RoutedEventArgs());
            var sb = new StringBuilder();
            sb.Append(mapContainer.GetStats());
            if (mapImage != null)
            {
                mapDisplay.SetImage(mapImage, true);
                //pictureBox.OffsetTileSelector = generatedOccluded ^ (mapImage.Height / TileSet.TILE_HEIGHT % 2 == 0);

                sb.AppendLine();
                sb.AppendLine("--Image--");
                sb.AppendLine($"Image size: {mapImage.Width}x{mapImage.Height}");
                sb.Append($"Memory size: {BytesFormatter.GetBytesReadable(mapImage.Bytes.Length * 4)}");
            }
            ProgressBar.Text = "Completed";
            StatsTextBox.Text = sb.ToString();
            SaveAsImageButton.IsEnabled = true;
            mapViewerController.EvenTiles = mapContainer.Model.TiledMapSize.Height % 2 == 0;
        }

        private void ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            if (e.UserState is string text)
            {
                ProgressBar.Text = text;
            }
            if (e.UserState is WorkReporter.WorkerWarning warning)
            {
                MessageBox.Show(warning.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadMap(object? sender, DoWorkEventArgs e)
        {
            var workReporter = new WorkReporter(backgroundWorker, 5);
            ProgressBar.Maximum = workReporter.StagesLeft * 1000;
            if (mapContainer == null)
            {
                workReporter.StartNewStage(1, "Loading Map...");
                mapContainer = MapContainer.ReadMap(fs, filename, workReporter);

                workReporter.StartNewStage(2, "Loading GTL...");
                mapContainer.LoadGtlTiles(fs, workReporter);

                workReporter.StartNewStage(3, "Loading BTL...");
                mapContainer.LoadBtlTiles(fs, workReporter);

                workReporter.StartNewStage(4, "Loading external extra, monster, npc...");
                mapContainer.LoadExternal(fs, workReporter);
            }

            workReporter.StartNewStage(5, "Generating map...");
            var mapGenerator = new MapImageGenerator(workReporter, mapContainer, textGenerator);

            mapImage = mapGenerator.GenerateMap(checkboxOptions);
        }

        private GeneratorOptions CheckBoxesToGenerationOptions()
        {
            return new GeneratorOptions
               (
                   generatedOccluded,
                   GtlCheckBox.IsChecked ?? false,
                   SpritesCheckBox.IsChecked ?? false,
                   CollisionsCheckBox.IsChecked ?? false,
                   BtlCheckBox.IsChecked ?? false,
                   RoofsCheckBox.IsChecked ?? false,
                   EventsCheckBox.IsChecked ?? false,
                   ExtraCheckBox.IsChecked ?? false,
                   MonsterCheckBox.IsChecked ?? false,
                   NpcCheckBox.IsChecked ?? false,
                   DebugDotsCheckBox.IsChecked ?? false
               );
        }

        private void ShowTile(TileSet.Tile tile)
        {
            sidePreviewImage = new RawRgb(TileSet.TILE_WIDTH, TileSet.TILE_HEIGHT);
            tile.PlotTileOnBitmap(sidePreviewImage, 0, 0);
            //TileDisplayer.SetImage(sidePreviewImage.Bitmap, true);
        }

        // Debug function
        private void DebugClick(object sender, RoutedEventArgs e)
        {
            string[] files = fs.Directory.GetFiles(Settings.GameRootDir + @"\Map\Multi", "*.mon");
            using (var worker = new BackgroundWorker())
            {
                var results = new Dictionary<string, List<(int id, int count)>>();
                foreach (string file in files)
                {
                    var fileFingings = new List<int>();
                    var lines = fs.File.ReadAllLines(file);
                    foreach (var line in lines)
                    {
                        if (!line.StartsWith(";"))
                        {
                            var id = int.Parse(line.Split(',')[1]);
                            if (id > 36) fileFingings.Add(id);
                        }
                    }
                    if (fileFingings.Count > 0)
                        results[fs.Path.GetFileNameWithoutExtension(file)] = fileFingings.GroupBy(x => x)
                            .Select(g => (g.Key, g.Count())).ToList();
                }
                var json = string.Join(",", results.Select(entry => $"\"{entry.Key}\" : [{string.Join(",", entry.Value.Select(g => $"{{ \"id\":{g.id}, \"count\":{g.count}}}").ToArray())}]").ToArray());
            }
        }

        public void Close()
        {
            if (mapContainer != null) Settings.MapGenerationOptions = CheckBoxesToGenerationOptions();
            Settings.CopyCommandToClipboard = ToggleClipBoard.IsChecked ?? false;
        }
    }
}

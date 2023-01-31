using DispelTools.DataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using View.Views;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private INestedView? nestedControl;

        private Dictionary<string, IViewInfo> AvailableViews = new();
        public MainWindow()
        {
            InitializeComponent();
            CreateViewList();
            MenuItem? settingMenuItem = FindMenuItem(TopMenu.Items, nameof(SettingsView));
            SetView(new SettingsView(), settingMenuItem);
        }
        void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        void OpenViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewName = e.Parameter as string;
            
            if (viewName is not null && AvailableViews.TryGetValue(viewName, out var viewInfo))
            {
                var viewConstructor = viewInfo.PrepareConstructor();
                if (viewConstructor is not null)
                {
                    var view = viewConstructor.Construct();
                    SetView(view, FindMenuItem(TopMenu.Items, viewName));
                }
                else
                {
                    if(viewConstructor is null)
                    {
                        MessageBox.Show($"Cannot construct view with name {viewName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show($"Cannot find view with name {viewName}");
            }
        }

        private void CreateViewList()
        {
            AddSimpleViewInfo<SettingsView>();
            AddSimpleViewInfo<SimpleEditorView>();
            AddSingleParamViewInfo<ExtractorView, DispelTools.DataExtractor.ImageExtractor.SprImageExtractorFactory>("Sprite");
            AddSingleParamViewInfo<ExtractorView, DispelTools.DataExtractor.MapExtractor.MapImageExtractorFactory>("Map");
            AddSingleParamViewInfo<ExtractorView, DispelTools.DataExtractor.SoundExtractor.SnfSoundExtractorFactory>("Sound");
            AddSingleParamViewInfo<ExtractorView, DispelTools.DataExtractor.AllExtractor.AllFilesExtractorFactory>("All");
        }

        private void AddSimpleViewInfo<T>() where T : INestedView
        {
            AvailableViews[typeof(T).Name] = new ViewInfo<T>();
        }
        private void AddSingleParamViewInfo<T,K>(string secondParam) where T : INestedView
        {
            AvailableViews[$"{typeof(T).Name} {secondParam}"] = new ViewInfo<T>(typeof(K));
        }

        private void SetView(INestedView? view, MenuItem? menuItem)
        {
            if (view is not null && view is UserControl control)
            {
                UncheckAll(TopMenu.Items);
                if (menuItem is not null) menuItem.IsChecked = true;

                Contents.Content = control;
                nestedControl?.Close();
                nestedControl = view;
            }
        }

        private MenuItem? FindMenuItem(ItemCollection menuItems, string commandName)
        {
            foreach (var item in menuItems)
            {
                if (item is MenuItem menu)
                {
                    if ((string)menu.CommandParameter == commandName)
                        return menu;
                    else
                    {
                        if (menu.Items.Count > 0)
                        {
                            var found = FindMenuItem(menu.Items, commandName);
                            if (found is not null) return found;
                        }
                    }
                }
            }
            return null;
        }

        private void UncheckAll(ItemCollection menuItems)
        {
            foreach (var item in menuItems)
            {
                if (item is MenuItem menu)
                {
                    if (menu.IsChecked)
                        menu.IsChecked = false;
                    else
                        UncheckAll(menu.Items);
                }
            }
        }
    }
}

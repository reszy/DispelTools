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
        public MainWindow()
        {
            InitializeComponent();
            MenuItem? settingMenuItem = FindMenuItem(TopMenu.Items, nameof(SettingsView));
            SetView(new SettingsView(), settingMenuItem);
        }
        void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        void OpenViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is not MenuItem) return;

            var viewName = e.Parameter as string;
            if (viewName is null) return;

            var viewType = Type.GetType($"View.Views.{viewName}");
            if (viewType?.IsAssignableTo(typeof(INestedView)) ?? false)
            {
                var viewConstructor = viewType.GetConstructor(Array.Empty<Type>());

                if (e.OriginalSource is MenuItem menuItem && viewConstructor is not null)
                {
                    var view = viewConstructor.Invoke(Array.Empty<object>()) as INestedView;
                    SetView(view, menuItem);
                }
            }
            else
            {
                MessageBox.Show($"Cannot find view with name {viewName}");
            }
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

                Width = control.Width;
                ContentsHeight.Height = new(control.Height);
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

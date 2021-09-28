using System.Collections.Generic;
using System.Windows.Forms;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public partial class CustomPropertyGrid : UserControl
    {
        private PropertyItem selectedItem;
        private readonly List<Row> rows = new List<Row>();

        public CustomPropertyGrid()
        {
            InitializeComponent();
        }

        public PropertyItem SelectedItem { get => selectedItem; set => SelectItem(value); }
        public bool HideUnnamedFields { get; internal set; }

        private void SelectItem(PropertyItem propertyItem)
        {
            BeginControlUpdate();
            SuspendLayout();

            ClearFields();
            selectedItem = propertyItem;
            if (propertyItem != null)
            {
                for (int i = 0, shownFieldCounter = 0; i < propertyItem.Count; i++)
                {
                    if (!HideUnnamedFields || !selectedItem[i].Name.StartsWith("?"))
                    {
                        var row = new Row(selectedItem, i, shownFieldCounter, this);
                        rows.Add(row);
                        shownFieldCounter++;
                    }
                }
            }
            EndControlUpdate();
            ResumeLayout(true);
        }


        private void ClearFields()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].DisposeAll();
            }
            rows.Clear();
            Controls.Clear();
        }

        //private void MouseWheelHandler(object sender, MouseEventArgs e) => AutoScrollPosition = new Point(AutoScrollPosition.X, AutoScrollPosition.Y / fieldHeight + (fieldHeight * Math.Sign(e.Delta)));
    }
}

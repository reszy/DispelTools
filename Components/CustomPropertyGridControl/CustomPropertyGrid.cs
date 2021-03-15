using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public partial class CustomPropertyGrid : UserControl
    {
        private PropertyItem selectedItem;


        public CustomPropertyGrid()
        {
            InitializeComponent();
        }


        public PropertyItem SelectedItem { get => selectedItem; set => SelectItem(value); }

        private void SelectItem(PropertyItem propertyItem)
        {
            BeginControlUpdate();
            SuspendLayout();

            ClearFields();
            selectedItem = propertyItem;
            if (propertyItem != null)
            {
                for (int i = 0; i< propertyItem.Count; i++) {
                    var row = new Row(ref selectedItem, i, this);
                    Controls.Add(row);
                }
            }
            EndControlUpdate();
            ResumeLayout(true);
        }


        private void ClearFields()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                (Controls[i] as Row).DisposeAll();
            }
            Controls.Clear();
        }

        //private void MouseWheelHandler(object sender, MouseEventArgs e) => AutoScrollPosition = new Point(AutoScrollPosition.X, AutoScrollPosition.Y / fieldHeight + (fieldHeight * Math.Sign(e.Delta)));
    }
}

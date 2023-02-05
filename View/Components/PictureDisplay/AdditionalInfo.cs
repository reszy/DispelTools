using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace View.Components.PictureDisplay
{
    public class AdditionalInfo
    {
        public string[] Labels { get; }
        public List<string[]> Info { get; }
        public byte InfoSize { get; }

        public AdditionalInfo()
        {
            InfoSize = 0;
            Labels = Array.Empty<string>();
            Info = new();
        }

        public AdditionalInfo(string label1, string label2, string label3, string label4)
        {
            InfoSize = 4;
            Labels = new string[] { label1, label2, label3, label4 };
            Info = new();
        }
        public AdditionalInfo(string label1, string label2, string label3)
        {
            InfoSize = 3;
            Labels = new string[] { label1, label2, label3 };
            Info = new();
        }
        public AdditionalInfo(string label1, string label2)
        {
            InfoSize = 2;
            Labels = new string[] { label1, label2 };
            Info = new();
        }
        public AdditionalInfo(string label1)
        {
            InfoSize = 1;
            Labels = new string[] { label1 };
            Info = new();
        }

        public void Add(string field1, string field2, string field3, string field4)
        {
            if (InfoSize != 4) throw new ArgumentException("Tried add wrong number of fields of info. Expected 4");
            Info.Add(new string[] { field1, field2, field3, field4 });
        }
        public void Add(string field1, string field2, string field3)
        {
            if (InfoSize != 3) throw new ArgumentException("Tried add wrong number of fields of info. Expected 3");
            Info.Add(new string[] { field1, field2, field3 });
        }
        public void Add(string field1, string field2)
        {
            if (InfoSize != 2) throw new ArgumentException("Tried add wrong number of fields of info. Expected 2");
            Info.Add(new string[] { field1, field2 });
        }
        public void Add(string field1)
        {
            if (InfoSize != 1) throw new ArgumentException("Tried add wrong number of fields of info. Expected 1");
            Info.Add(new string[] { field1 });
        }

        public IEnumerable<UIElement> ToUiElements()
        {
            foreach (var info in Info)
            {
                yield return ToGrid(info);
            }
        }

        private Grid ToGrid(string[] info)
        {
            var grid = new Grid();
            for (int i = 0; i < 4; i++) grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            GetLabelWithText(info, grid);
            return grid;
        }

        private void GetLabelWithText(string[] info, Grid grid)
        {
            for (int i = 0; i < InfoSize; i++)
            {
                var lwt = new LabelWithValue() { LabelText = Labels[i], ValueText = info[i] };
                Grid.SetRow(grid, i);
                grid.Children.Add(lwt);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.Components
{
    public partial class DetailsComponent : UserControl
    {
        private string header = "Details";
        private int unfoldSize = 100;
        private bool folded = true;

        public DetailsComponent()
        {
            InitializeComponent();
            contentBox.Text = "";
            headerArrowLabel.Text = "▼";
        }

        private void headerPanel_Click(object sender, EventArgs e)
        {
            if (folded)
            {
                headerArrowLabel.Text = "▲";
                Size = new Size(Size.Width, unfoldSize);
            }
            else
            {
                headerArrowLabel.Text = "▼";
                Size = new Size(Size.Width, 1);
            }
            folded = !folded;
        }

        public string Header { get => header; set { header = value; headerLabel.Text = value; } }
        public int UnfoldSize { get => unfoldSize; set { unfoldSize = value; contentBox.Size = new Size(contentBox.Size.Width, unfoldSize); } }


        public void AddDetails(List<string> details)
        {
            //detailsContetns.AddRange(details);
            foreach (var detail in details)
            {
                contentBox.AppendText(detail + "\r\n");
            }
        }
        public void AddDetails(string details)
        {
            contentBox.AppendText(details + "\r\n");
        }
        public void ClearDetails()
        {
            contentBox.Clear();
        }
    }
}

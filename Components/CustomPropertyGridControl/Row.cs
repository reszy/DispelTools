using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public partial class Row : UserControl
    {
        private readonly PropertyItem all;
        private int number;
        private readonly Field field;
        private readonly int fieldHeight;
        private readonly Font bolden;
        private readonly Label label;
        private readonly Button resetButton;
        private readonly NumericUpDown numericControl;
        private readonly TextBox textControl;
        private readonly ToolTip toolTip;

        private Field FieldRef => all[number];
        public Row(ref PropertyItem item, int number, UserControl parent)
        {
            all = item;
            this.number = number;
            field = item[number];
            bolden = new Font(Font, FontStyle.Bold);
            fieldHeight = Font.Height + 6;
            toolTip = new ToolTip();

            InitializeComponent();
            Location = new Point(parent.AutoScrollPosition.X, fieldHeight * number + parent.AutoScrollPosition.Y);
            Size = new Size(parent.Size.Width - 4, fieldHeight);
            Anchor = AnchorStyles.Top | AnchorStyles.Left;

            SuspendLayout();

            {
                var controlSize = new Size(Size.Width / 2, fieldHeight);
                var resetButtonSize = new Size(Font.Height + 3, fieldHeight - 2);
                var labelPosition = new Point(0, 0);
                var labelSize = new Size(controlSize.Width - (field.ReadOnly ? 0 : resetButtonSize.Width), fieldHeight);
                var resetButtonPosition = new Point(labelSize.Width, 0);
                var controlPosition = new Point(controlSize.Width, 0);

                label = new Label()
                {
                    Name = field.Name + "Label",
                    Text = field.Name,
                    Location = labelPosition,
                    Size = labelSize,
                    BorderStyle = BorderStyle.None,
                    BackColor = SystemColors.Control,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left,
                    AutoEllipsis = true
                };
                toolTip.SetToolTip(label, field.Name);
                Controls.Add(label);

                if (!field.ReadOnly)
                {
                    resetButton = new Button()
                    {
                        Name = field.Name + "ResetButton",
                        Text = GetResetButtonText(),
                        Font = new Font(Font.FontFamily, Font.Size - 3),
                        Location = resetButtonPosition,
                        Size = resetButtonSize,
                        FlatStyle = FlatStyle.Standard,
                        BackColor = SystemColors.ButtonFace,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    resetButton.Click += ResetValue;
                    Controls.Add(resetButton);
                    toolTip.SetToolTip(resetButton, "Reset value");
                }

                if (field.Type != Field.FieldType.ASCII)
                {
                    textControl = null;
                    numericControl = new NumericUpDown()
                    {
                        Name = field.Name,
                        Maximum = int.MaxValue,
                        Minimum = int.MinValue,
                        Value = new decimal((int)field.Value),
                        Location = controlPosition,
                        Size = controlSize,
                        Enabled = !field.ReadOnly,
                        Hexadecimal = field.Type == Field.FieldType.HEX,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    numericControl.ValueChanged += NumberValueChanged;
                    Controls.Add(numericControl);
                }
                else
                {
                    numericControl = null;
                    textControl = new TextBox()
                    {
                        Name = field.Name,
                        Text = field.Value.ToString(),
                        Location = controlPosition,
                        Size = controlSize,
                        Enabled = !field.ReadOnly,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    textControl.TextChanged += TextValueChanged;
                    Controls.Add(textControl);
                }

            }
            ResumeLayout(false);
        }

        private string GetResetButtonText() => !Equals(FieldRef.Value, FieldRef.DefaultValue) ? "●" : "○";

        private void TextValueChanged(object sender, EventArgs e)
        {
            if (textControl.Text == field.DefaultValue.ToString())
            {
                textControl.Font = Font;
            }
            else
            {
                textControl.Font = bolden;
            }
            FieldRef.Value = textControl.Text;
            resetButton.Text = GetResetButtonText();
        }

        private void NumberValueChanged(object sender, EventArgs e)
        {
            if (numericControl.Value == new decimal((int)field.DefaultValue))
            {
                numericControl.Font = Font;
            }
            else
            {
                numericControl.Font = bolden;
            }
            FieldRef.Value = (int)numericControl.Value;
            resetButton.Text = GetResetButtonText();
        }

        private void ResetValue(object sender, EventArgs e)
        {
            if (!field.ReadOnly)
            {
                if (numericControl != null)
                {
                    numericControl.Value = new decimal((int)field.DefaultValue);
                    NumberValueChanged(sender, EventArgs.Empty);
                }
                if (textControl != null)
                {
                    textControl.Text = field.DefaultValue.ToString();
                    TextValueChanged(sender, EventArgs.Empty);
                }
            }
        }

        public void DisposeAll()
        {
            label.Dispose();
            resetButton?.Dispose();
            textControl?.Dispose();
            numericControl?.Dispose();
            toolTip?.Dispose();
            bolden.Dispose();
        }
    }

}

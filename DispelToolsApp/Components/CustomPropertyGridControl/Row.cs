using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public class Row
    {
        private PropertyItem all;
        private CustomPropertyGrid parent;

        private int number;
        private readonly Field field;
        private readonly int fieldHeight;
        private readonly Font bolden;
        private readonly Label label;
        private readonly Button resetButton;
        private readonly NumericUpDown numericControl;
        private readonly TextBox textControl;

        private Field FieldRef => all[number];

        public Point Location { get; set; }
        public Size Size{ get; set; }
        public Font Font { get; set; }
        public Row(ref PropertyItem item, int number, int locationNumber, CustomPropertyGrid parent)
        {
            all = item;
            this.parent = parent;
            this.number = number;
            field = item[number];
            Font = parent.Font;
            bolden = new Font(Font, FontStyle.Bold);
            fieldHeight = Font.Height + 6;

            Location = new Point(parent.AutoScrollPosition.X, fieldHeight * locationNumber + parent.AutoScrollPosition.Y);
            Size = new Size(parent.Size.Width - 4 - parent.AutoScrollMinSize.Width, fieldHeight);

            {
                var controlSize = new Size(Size.Width / 2, fieldHeight);
                var resetButtonSize = new Size(Font.Height + 3, fieldHeight - 2);
                var labelPosition = Location;
                var labelSize = new Size(controlSize.Width - (field.ReadOnly ? 0 : resetButtonSize.Width), fieldHeight);
                var resetButtonPosition = new Point(Location.X + labelSize.Width, Location.Y);
                var controlPosition = new Point(Location.X + controlSize.Width, Location.Y);

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
                parent.toolTip.SetToolTip(label, field.Name + "\n" + field.Description);
                parent.Controls.Add(label);

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
                    parent.Controls.Add(resetButton);
                    parent.toolTip.SetToolTip(resetButton, "Reset value");
                }

                if (field.IsText)
                {
                    numericControl = null;
                    textControl = new TextBox()
                    {
                        Name = field.Name,
                        Text = field.EncodedText,
                        Location = controlPosition,
                        Size = controlSize,
                        Enabled = !field.ReadOnly,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    textControl.TextChanged += TextValueChanged;
                    parent.Controls.Add(textControl);
                }
                else
                {
                    textControl = null;
                    numericControl = new NumericUpDown()
                    {
                        Name = field.Name,
                        Maximum = field.MaxValue,
                        Minimum = field.MinValue,
                        Value = field.DecimalValue,
                        Location = controlPosition,
                        Size = controlSize,
                        Enabled = !field.ReadOnly,
                        Hexadecimal = field.Type == Field.DisplayType.HEX,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    numericControl.ValueChanged += NumberValueChanged;
                    parent.Controls.Add(numericControl);
                }
            }
        }

        private string GetResetButtonText() => FieldRef.HasChanged ? "●" : "○";

        private void TextValueChanged(object sender, EventArgs e)
        {
            FieldRef.SetValue(textControl.Text);
            textControl.Font = FieldRef.HasChanged ? bolden : Font;
            resetButton.Text = GetResetButtonText();
        }

        private void NumberValueChanged(object sender, EventArgs e)
        {
            FieldRef.SetValue(numericControl.Value);
            numericControl.Font = FieldRef.HasChanged ? bolden : Font;
            resetButton.Text = GetResetButtonText();
        }

        private void ResetValue(object sender, EventArgs e)
        {
            if (!field.ReadOnly)
            {
                field.RevertValue();
                if (numericControl != null)
                {
                    numericControl.Value = field.DecimalValue;
                    numericControl.Font = field.HasChanged ? bolden : Font;
                }
                if (textControl != null)
                {
                    textControl.Text = field.EncodedText;
                    textControl.Font = field.HasChanged ? bolden : Font;
                }
                resetButton.Text = GetResetButtonText();
            }
        }

        public void DisposeAll()
        {
            parent.toolTip.SetToolTip(label,null);
            if (resetButton != null)
            {
                parent.toolTip.SetToolTip(resetButton, null);
            }
            label.Dispose();
            resetButton?.Dispose();
            textControl?.Dispose();
            numericControl?.Dispose();
            bolden.Dispose();
            parent = null;
            all = null;
        }
    }

}

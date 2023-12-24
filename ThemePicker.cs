using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class ThemePicker : Form
    {
        public Color DarkSquares;
        public Color WhiteSquares;
        private Color DefaulDarkSquares;
        private Color DefaulWhiteSquares;

        public ThemePicker(Color darkSquares, Color whiteSquares)
        {
            InitializeComponent();

            this.BringToFront();

            this.DarkSquares = darkSquares;
            this.WhiteSquares = whiteSquares;
            this.DefaulDarkSquares = darkSquares;
            this.DefaulWhiteSquares = whiteSquares;
            this.ControlBox = false;

            this.BackColor = Color.FromArgb(73, 80, 87);

            UpdateCurrentColorPanels();
            foreach(var control in this.Controls)
            {
                if(control is Panel panel)
                {
                    if(panel.Name == "WhiteSquaresPicker")
                    {
                        panel.Click += WhiteSquaresPicker_Click;
                    }
                    else if(panel.Name == "DarkSquaresPicker")
                    {
                        panel.Click += BlackSquaresPicker_Click;
                    }
                }

                if(control is Button button)
                {
                    button.BackColor = Color.FromArgb(222, 226, 230);
                    button.ForeColor = Color.FromArgb(33, 37, 41);
                    button.Font = new Font(button.Font, FontStyle.Bold);

                   
                }else if(control is Label label)
                {
                    label.ForeColor = Color.FromArgb(248, 249, 250);
                }
            }
        }

        private void BlackSquaresPicker_Click(object sender, EventArgs e) => PanelColorChanger(false);
        private void WhiteSquaresPicker_Click(object sender, EventArgs e) => PanelColorChanger(true);        

        private void PanelColorChanger(bool white)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    if(white) WhiteSquares = colorDialog.Color;
                    else DarkSquares = colorDialog.Color;
                }
            }

            UpdateCurrentColorPanels();
        }

        private void UpdateCurrentColorPanels()
        {
            WhiteSquaresPicker.BackColor = WhiteSquares;
            DarkSquaresPicker.BackColor = DarkSquares;
        }

        private void BlueTheme_Click(object sender, EventArgs e)
        {
            DarkSquares = Color.FromArgb(90, 126, 176);
            WhiteSquares = Color.FromArgb(250, 246, 202);

            UpdateCurrentColorPanels();
        }

        private void GreenTheme_Click(object sender, EventArgs e)
        {
            DarkSquares = Color.FromArgb(109, 139, 70);
            WhiteSquares = Color.FromArgb(235, 236, 208);

            UpdateCurrentColorPanels();
        }

        private void BrownTheme_Click(object sender, EventArgs e)
        {
            DarkSquares = Color.FromArgb(96, 69, 63);
            WhiteSquares = Color.FromArgb(209, 193, 167);

            UpdateCurrentColorPanels();
        }

        private void GrayTheme_Click(object sender, EventArgs e)
        {
            DarkSquares = Color.FromArgb(127, 127, 126);
            WhiteSquares = Color.FromArgb(240, 240, 240);

            UpdateCurrentColorPanels();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DarkSquares = DefaulDarkSquares;
            WhiteSquares = DefaulWhiteSquares;

            this.Close();
        }
    }
}

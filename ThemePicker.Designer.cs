namespace Chess
{
    partial class ThemePicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.WhiteSquaresPicker = new System.Windows.Forms.Panel();
            this.DarkSquaresPicker = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.BlueTheme = new System.Windows.Forms.Button();
            this.BrownTheme = new System.Windows.Forms.Button();
            this.GreenTheme = new System.Windows.Forms.Button();
            this.GrayTheme = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 33);
            this.label1.TabIndex = 2;
            this.label1.Text = "Theme Customization";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Custom: ";
            // 
            // WhiteSquaresPicker
            // 
            this.WhiteSquaresPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WhiteSquaresPicker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WhiteSquaresPicker.Location = new System.Drawing.Point(194, 120);
            this.WhiteSquaresPicker.Name = "WhiteSquaresPicker";
            this.WhiteSquaresPicker.Size = new System.Drawing.Size(45, 45);
            this.WhiteSquaresPicker.TabIndex = 4;
            // 
            // DarkSquaresPicker
            // 
            this.DarkSquaresPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DarkSquaresPicker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DarkSquaresPicker.Location = new System.Drawing.Point(396, 120);
            this.DarkSquaresPicker.Name = "DarkSquaresPicker";
            this.DarkSquaresPicker.Size = new System.Drawing.Size(45, 45);
            this.DarkSquaresPicker.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(43, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Known Themes:";
            // 
            // BlueTheme
            // 
            this.BlueTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BlueTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BlueTheme.Location = new System.Drawing.Point(59, 391);
            this.BlueTheme.Name = "BlueTheme";
            this.BlueTheme.Size = new System.Drawing.Size(195, 45);
            this.BlueTheme.TabIndex = 7;
            this.BlueTheme.Text = "Blue";
            this.BlueTheme.UseVisualStyleBackColor = true;
            this.BlueTheme.Click += new System.EventHandler(this.BlueTheme_Click);
            // 
            // BrownTheme
            // 
            this.BrownTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BrownTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrownTheme.Location = new System.Drawing.Point(59, 312);
            this.BrownTheme.Name = "BrownTheme";
            this.BrownTheme.Size = new System.Drawing.Size(195, 45);
            this.BrownTheme.TabIndex = 8;
            this.BrownTheme.Text = "Brown";
            this.BrownTheme.UseVisualStyleBackColor = true;
            this.BrownTheme.Click += new System.EventHandler(this.BrownTheme_Click);
            // 
            // GreenTheme
            // 
            this.GreenTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GreenTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GreenTheme.Location = new System.Drawing.Point(59, 238);
            this.GreenTheme.Name = "GreenTheme";
            this.GreenTheme.Size = new System.Drawing.Size(195, 45);
            this.GreenTheme.TabIndex = 9;
            this.GreenTheme.Text = "Default";
            this.GreenTheme.UseVisualStyleBackColor = true;
            this.GreenTheme.Click += new System.EventHandler(this.GreenTheme_Click);
            // 
            // GrayTheme
            // 
            this.GrayTheme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GrayTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GrayTheme.Location = new System.Drawing.Point(59, 469);
            this.GrayTheme.Name = "GrayTheme";
            this.GrayTheme.Size = new System.Drawing.Size(195, 45);
            this.GrayTheme.TabIndex = 10;
            this.GrayTheme.Text = "Gray";
            this.GrayTheme.UseVisualStyleBackColor = true;
            this.GrayTheme.Click += new System.EventHandler(this.GrayTheme_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(68, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 45);
            this.label7.TabIndex = 16;
            this.label7.Text = "White Squares";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(265, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 45);
            this.label8.TabIndex = 17;
            this.label8.Text = "Dark Squares";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ApplyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyBtn.Location = new System.Drawing.Point(555, 593);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 34);
            this.ApplyBtn.TabIndex = 18;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Location = new System.Drawing.Point(654, 593);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 34);
            this.CancelBtn.TabIndex = 19;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ThemePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(782, 653);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.GrayTheme);
            this.Controls.Add(this.GreenTheme);
            this.Controls.Add(this.BrownTheme);
            this.Controls.Add(this.BlueTheme);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DarkSquaresPicker);
            this.Controls.Add(this.WhiteSquaresPicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(800, 700);
            this.MinimumSize = new System.Drawing.Size(800, 700);
            this.Name = "ThemePicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Theme Customization";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel WhiteSquaresPicker;
        private System.Windows.Forms.Panel DarkSquaresPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BlueTheme;
        private System.Windows.Forms.Button BrownTheme;
        private System.Windows.Forms.Button GreenTheme;
        private System.Windows.Forms.Button GrayTheme;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}
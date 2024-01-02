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
            label1 = new Label();
            label2 = new Label();
            WhiteSquaresPicker = new Panel();
            DarkSquaresPicker = new Panel();
            label3 = new Label();
            BlueTheme = new Button();
            BrownTheme = new Button();
            GreenTheme = new Button();
            GrayTheme = new Button();
            label7 = new Label();
            label8 = new Label();
            ApplyBtn = new Button();
            CancelBtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 16.2F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(30, 32);
            label1.Name = "label1";
            label1.Size = new Size(309, 33);
            label1.TabIndex = 2;
            label1.Text = "Theme Customization";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(43, 99);
            label2.Name = "label2";
            label2.Size = new Size(81, 19);
            label2.TabIndex = 3;
            label2.Text = "Custom: ";
            // 
            // WhiteSquaresPicker
            // 
            WhiteSquaresPicker.BorderStyle = BorderStyle.FixedSingle;
            WhiteSquaresPicker.Cursor = Cursors.Hand;
            WhiteSquaresPicker.Location = new Point(194, 150);
            WhiteSquaresPicker.Margin = new Padding(3, 4, 3, 4);
            WhiteSquaresPicker.Name = "WhiteSquaresPicker";
            WhiteSquaresPicker.Size = new Size(60, 56);
            WhiteSquaresPicker.TabIndex = 4;
            // 
            // DarkSquaresPicker
            // 
            DarkSquaresPicker.BorderStyle = BorderStyle.FixedSingle;
            DarkSquaresPicker.Cursor = Cursors.Hand;
            DarkSquaresPicker.Location = new Point(396, 150);
            DarkSquaresPicker.Margin = new Padding(3, 4, 3, 4);
            DarkSquaresPicker.Name = "DarkSquaresPicker";
            DarkSquaresPicker.Size = new Size(58, 56);
            DarkSquaresPicker.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 10.2F, FontStyle.Bold);
            label3.Location = new Point(43, 236);
            label3.Name = "label3";
            label3.Size = new Size(136, 19);
            label3.TabIndex = 6;
            label3.Text = "Known Themes:";
            // 
            // BlueTheme
            // 
            BlueTheme.Cursor = Cursors.Hand;
            BlueTheme.FlatStyle = FlatStyle.Flat;
            BlueTheme.Location = new Point(59, 489);
            BlueTheme.Margin = new Padding(3, 4, 3, 4);
            BlueTheme.Name = "BlueTheme";
            BlueTheme.Size = new Size(195, 56);
            BlueTheme.TabIndex = 7;
            BlueTheme.Text = "Blue";
            BlueTheme.UseVisualStyleBackColor = true;
            BlueTheme.Click += BlueTheme_Click;
            // 
            // BrownTheme
            // 
            BrownTheme.Cursor = Cursors.Hand;
            BrownTheme.FlatStyle = FlatStyle.Flat;
            BrownTheme.Location = new Point(59, 390);
            BrownTheme.Margin = new Padding(3, 4, 3, 4);
            BrownTheme.Name = "BrownTheme";
            BrownTheme.Size = new Size(195, 56);
            BrownTheme.TabIndex = 8;
            BrownTheme.Text = "Brown";
            BrownTheme.UseVisualStyleBackColor = true;
            BrownTheme.Click += BrownTheme_Click;
            // 
            // GreenTheme
            // 
            GreenTheme.Cursor = Cursors.Hand;
            GreenTheme.FlatStyle = FlatStyle.Flat;
            GreenTheme.Location = new Point(59, 298);
            GreenTheme.Margin = new Padding(3, 4, 3, 4);
            GreenTheme.Name = "GreenTheme";
            GreenTheme.Size = new Size(195, 56);
            GreenTheme.TabIndex = 9;
            GreenTheme.Text = "Default";
            GreenTheme.UseVisualStyleBackColor = true;
            GreenTheme.Click += GreenTheme_Click;
            // 
            // GrayTheme
            // 
            GrayTheme.Cursor = Cursors.Hand;
            GrayTheme.FlatStyle = FlatStyle.Flat;
            GrayTheme.Location = new Point(59, 586);
            GrayTheme.Margin = new Padding(3, 4, 3, 4);
            GrayTheme.Name = "GrayTheme";
            GrayTheme.Size = new Size(195, 56);
            GrayTheme.TabIndex = 10;
            GrayTheme.Text = "Gray";
            GrayTheme.UseVisualStyleBackColor = true;
            GrayTheme.Click += GrayTheme_Click;
            // 
            // label7
            // 
            label7.Font = new Font("Arial", 7.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label7.Location = new Point(68, 150);
            label7.Name = "label7";
            label7.Size = new Size(120, 56);
            label7.TabIndex = 16;
            label7.Text = "White Squares";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.Font = new Font("Arial", 7.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label8.Location = new Point(265, 150);
            label8.Name = "label8";
            label8.Size = new Size(125, 56);
            label8.TabIndex = 17;
            label8.Text = "Dark Squares";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ApplyBtn
            // 
            ApplyBtn.Cursor = Cursors.Hand;
            ApplyBtn.FlatStyle = FlatStyle.Flat;
            ApplyBtn.Location = new Point(555, 741);
            ApplyBtn.Margin = new Padding(3, 4, 3, 4);
            ApplyBtn.Name = "ApplyBtn";
            ApplyBtn.Size = new Size(75, 42);
            ApplyBtn.TabIndex = 18;
            ApplyBtn.Text = "Apply";
            ApplyBtn.UseVisualStyleBackColor = true;
            ApplyBtn.Click += ApplyBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Cursor = Cursors.Hand;
            CancelBtn.FlatStyle = FlatStyle.Flat;
            CancelBtn.Location = new Point(654, 741);
            CancelBtn.Margin = new Padding(3, 4, 3, 4);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(75, 42);
            CancelBtn.TabIndex = 19;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // ThemePicker
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(782, 816);
            Controls.Add(CancelBtn);
            Controls.Add(ApplyBtn);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(GrayTheme);
            Controls.Add(GreenTheme);
            Controls.Add(BrownTheme);
            Controls.Add(BlueTheme);
            Controls.Add(label3);
            Controls.Add(DarkSquaresPicker);
            Controls.Add(WhiteSquaresPicker);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(800, 863);
            MinimumSize = new Size(800, 863);
            Name = "ThemePicker";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Theme Customization";
            ResumeLayout(false);
            PerformLayout();
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
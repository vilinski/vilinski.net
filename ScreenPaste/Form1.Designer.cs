using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenPaste
{
    partial class Form1
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
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.pictureBox3 = new PictureBox();
            this.label1 = new Label();
            this.pictureBox2 = new PictureBox();
            PictureBox pictureBox = new PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            pictureBox.Anchor = AnchorStyles.None;
            pictureBox.Cursor = Cursors.SizeAll;
            //pictureBox.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
            pictureBox.Location = new Point(88, 88);
            pictureBox.Margin = new Padding(0);
            pictureBox.Name = "pictureBox1";
            pictureBox.Size = new Size(20, 20);
            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            pictureBox.DoubleClick += new EventHandler(this.pictureBox1_DoubleClick);
            pictureBox.MouseDown += new MouseEventHandler(this.Form1_MouseDown);
            this.panel1.BackColor = Color.DarkSlateGray;
            this.panel1.Controls.Add((Control)this.panel2);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(200, 200);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new MouseEventHandler(this.panResize_MouseDown);
            this.panel1.MouseMove += new MouseEventHandler(this.panResize_MouseMove);
            this.panel1.MouseUp += new MouseEventHandler(this.panResize_MouseUp);
            this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panel2.BackColor = Color.SlateGray;
            this.panel2.Controls.Add((Control)this.pictureBox3);
            this.panel2.Controls.Add((Control)this.label1);
            this.panel2.Controls.Add((Control)pictureBox);
            this.panel2.Controls.Add((Control)this.pictureBox2);
            this.panel2.Location = new Point(2, 2);
            this.panel2.Margin = new Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(196, 196);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new PaintEventHandler(this.panel2_Paint);
            this.panel2.DoubleClick += new EventHandler(this.pictureBox1_DoubleClick);
            this.panel2.MouseDown += new MouseEventHandler(this.panResize_MouseDown);
            this.panel2.MouseMove += new MouseEventHandler(this.panResize_MouseMove);
            this.panel2.MouseUp += new MouseEventHandler(this.panResize_MouseUp);
            this.pictureBox3.Anchor = AnchorStyles.None;
            this.pictureBox3.Cursor = Cursors.Hand;
            //this.pictureBox3.Image = (Image)componentResourceManager.GetObject("pictureBox3.Image");
            this.pictureBox3.Location = new Point(66, 111);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Size(70, 24);
            this.pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new EventHandler(this.pictureBox1_DoubleClick);
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)204);
            this.label1.ForeColor = Color.FromArgb(12, 12, 12);
            this.label1.ImageKey = "(отсутствует)";
            this.label1.Location = new Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new Size(15, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "w";
            this.pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.pictureBox2.Cursor = Cursors.SizeAll;
            this.pictureBox2.Location = new Point(18, 18);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(163, 163);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
            this.pictureBox2.MouseDown += new MouseEventHandler(this.Form1_MouseDown);
            this.BackColor = Color.Black;
            this.ClientSize = new Size(200, 200);
            this.Controls.Add((Control)this.panel1);
            this.MinimumSize = new Size(100, 100);
            this.Name = "Form1";
            this.Opacity = 0.65;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TopMost = true;
            this.TransparencyKey = Color.Black;
            this.Load += new EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Point ResizeLocation = Point.Empty;
        private DateTime _timestamp;
        private string _fileName;
        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
    }
}


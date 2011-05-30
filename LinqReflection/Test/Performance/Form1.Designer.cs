namespace LinqReflection.PerformanceTest
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
		/// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise, <c>false</c>.</param>
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
			this.FPTPGetResults = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.FPGetResults = new System.Windows.Forms.TextBox();
			this.FPTPRunButton = new System.Windows.Forms.Button();
			this.FPRunButton = new System.Windows.Forms.Button();
			this.FPTPSetResults = new System.Windows.Forms.TextBox();
			this.FPSetResults = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.STGetResults = new System.Windows.Forms.TextBox();
			this.STSetResults = new System.Windows.Forms.TextBox();
			this.STDRunButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.NRunButton = new System.Windows.Forms.Button();
			this.NSetResults = new System.Windows.Forms.TextBox();
			this.NGetResults = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.MakeTextBox = new System.Windows.Forms.TextBox();
			this.MakeRunButton = new System.Windows.Forms.Button();
			this.DRunButton = new System.Windows.Forms.Button();
			this.DSetResults = new System.Windows.Forms.TextBox();
			this.DGetResults = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(134, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "FastProperty<T,TProperty>";
			// 
			// FPTPGetResults
			// 
			this.FPTPGetResults.Location = new System.Drawing.Point(113, 26);
			this.FPTPGetResults.Name = "FPTPGetResults";
			this.FPTPGetResults.Size = new System.Drawing.Size(100, 20);
			this.FPTPGetResults.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "FastProperty<o,o>";
			// 
			// FPGetResults
			// 
			this.FPGetResults.Location = new System.Drawing.Point(113, 52);
			this.FPGetResults.Name = "FPGetResults";
			this.FPGetResults.Size = new System.Drawing.Size(100, 20);
			this.FPGetResults.TabIndex = 3;
			// 
			// FPTPRunButton
			// 
			this.FPTPRunButton.Location = new System.Drawing.Point(326, 26);
			this.FPTPRunButton.Name = "FPTPRunButton";
			this.FPTPRunButton.Size = new System.Drawing.Size(75, 23);
			this.FPTPRunButton.TabIndex = 4;
			this.FPTPRunButton.Text = "Run";
			this.FPTPRunButton.UseVisualStyleBackColor = true;
			this.FPTPRunButton.Click += new System.EventHandler(this.FPTPRunButton_Click);
			// 
			// FPRunButton
			// 
			this.FPRunButton.Location = new System.Drawing.Point(326, 50);
			this.FPRunButton.Name = "FPRunButton";
			this.FPRunButton.Size = new System.Drawing.Size(75, 23);
			this.FPRunButton.TabIndex = 5;
			this.FPRunButton.Text = "Run";
			this.FPRunButton.UseVisualStyleBackColor = true;
			this.FPRunButton.Click += new System.EventHandler(this.FPRunButton_Click);
			// 
			// FPTPSetResults
			// 
			this.FPTPSetResults.Location = new System.Drawing.Point(220, 25);
			this.FPTPSetResults.Name = "FPTPSetResults";
			this.FPTPSetResults.Size = new System.Drawing.Size(100, 20);
			this.FPTPSetResults.TabIndex = 6;
			// 
			// FPSetResults
			// 
			this.FPSetResults.Location = new System.Drawing.Point(220, 52);
			this.FPSetResults.Name = "FPSetResults";
			this.FPSetResults.Size = new System.Drawing.Size(100, 20);
			this.FPSetResults.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(57, 81);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Standard";
			// 
			// STGetResults
			// 
			this.STGetResults.Location = new System.Drawing.Point(113, 78);
			this.STGetResults.Name = "STGetResults";
			this.STGetResults.Size = new System.Drawing.Size(100, 20);
			this.STGetResults.TabIndex = 9;
			// 
			// STSetResults
			// 
			this.STSetResults.Location = new System.Drawing.Point(220, 78);
			this.STSetResults.Name = "STSetResults";
			this.STSetResults.Size = new System.Drawing.Size(100, 20);
			this.STSetResults.TabIndex = 10;
			// 
			// STDRunButton
			// 
			this.STDRunButton.Location = new System.Drawing.Point(326, 76);
			this.STDRunButton.Name = "STDRunButton";
			this.STDRunButton.Size = new System.Drawing.Size(75, 23);
			this.STDRunButton.TabIndex = 11;
			this.STDRunButton.Text = "Run";
			this.STDRunButton.UseVisualStyleBackColor = true;
			this.STDRunButton.Click += new System.EventHandler(this.STDRunButton_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(149, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Get";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(256, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(23, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "Set";
			// 
			// NRunButton
			// 
			this.NRunButton.Location = new System.Drawing.Point(326, 128);
			this.NRunButton.Name = "NRunButton";
			this.NRunButton.Size = new System.Drawing.Size(75, 23);
			this.NRunButton.TabIndex = 17;
			this.NRunButton.Text = "Run";
			this.NRunButton.UseVisualStyleBackColor = true;
			this.NRunButton.Click += new System.EventHandler(this.NRunButton_Click);
			// 
			// NSetResults
			// 
			this.NSetResults.Location = new System.Drawing.Point(220, 130);
			this.NSetResults.Name = "NSetResults";
			this.NSetResults.Size = new System.Drawing.Size(100, 20);
			this.NSetResults.TabIndex = 16;
			// 
			// NGetResults
			// 
			this.NGetResults.Location = new System.Drawing.Point(113, 130);
			this.NGetResults.Name = "NGetResults";
			this.NGetResults.Size = new System.Drawing.Size(100, 20);
			this.NGetResults.TabIndex = 15;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(69, 133);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "Native";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(73, 159);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(34, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "Make";
			// 
			// MakeTextBox
			// 
			this.MakeTextBox.Location = new System.Drawing.Point(113, 156);
			this.MakeTextBox.Name = "MakeTextBox";
			this.MakeTextBox.Size = new System.Drawing.Size(207, 20);
			this.MakeTextBox.TabIndex = 19;
			// 
			// MakeRunButton
			// 
			this.MakeRunButton.Location = new System.Drawing.Point(326, 154);
			this.MakeRunButton.Name = "MakeRunButton";
			this.MakeRunButton.Size = new System.Drawing.Size(75, 23);
			this.MakeRunButton.TabIndex = 20;
			this.MakeRunButton.Text = "Run";
			this.MakeRunButton.UseVisualStyleBackColor = true;
			this.MakeRunButton.Click += new System.EventHandler(this.MakeRunButton_Click);
			// 
			// DRunButton
			// 
			this.DRunButton.Location = new System.Drawing.Point(326, 102);
			this.DRunButton.Name = "DRunButton";
			this.DRunButton.Size = new System.Drawing.Size(75, 23);
			this.DRunButton.TabIndex = 24;
			this.DRunButton.Text = "Run";
			this.DRunButton.UseVisualStyleBackColor = true;
			this.DRunButton.Click += new System.EventHandler(this.DRunButton_Click);
			// 
			// DSetResults
			// 
			this.DSetResults.Location = new System.Drawing.Point(220, 104);
			this.DSetResults.Name = "DSetResults";
			this.DSetResults.Size = new System.Drawing.Size(100, 20);
			this.DSetResults.TabIndex = 23;
			// 
			// DGetResults
			// 
			this.DGetResults.Location = new System.Drawing.Point(113, 104);
			this.DGetResults.Name = "DGetResults";
			this.DGetResults.Size = new System.Drawing.Size(100, 20);
			this.DGetResults.TabIndex = 22;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(59, 107);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 13);
			this.label8.TabIndex = 21;
			this.label8.Text = "Dynamic";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(410, 209);
			this.Controls.Add(this.DRunButton);
			this.Controls.Add(this.DSetResults);
			this.Controls.Add(this.DGetResults);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.MakeRunButton);
			this.Controls.Add(this.MakeTextBox);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.NRunButton);
			this.Controls.Add(this.NSetResults);
			this.Controls.Add(this.NGetResults);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.STDRunButton);
			this.Controls.Add(this.STSetResults);
			this.Controls.Add(this.STGetResults);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.FPSetResults);
			this.Controls.Add(this.FPTPSetResults);
			this.Controls.Add(this.FPRunButton);
			this.Controls.Add(this.FPTPRunButton);
			this.Controls.Add(this.FPGetResults);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.FPTPGetResults);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox FPTPGetResults;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox FPGetResults;
		private System.Windows.Forms.Button FPTPRunButton;
		private System.Windows.Forms.Button FPRunButton;
		private System.Windows.Forms.TextBox FPTPSetResults;
		private System.Windows.Forms.TextBox FPSetResults;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox STGetResults;
		private System.Windows.Forms.TextBox STSetResults;
		private System.Windows.Forms.Button STDRunButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button NRunButton;
		private System.Windows.Forms.TextBox NSetResults;
		private System.Windows.Forms.TextBox NGetResults;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox MakeTextBox;
		private System.Windows.Forms.Button MakeRunButton;
		private System.Windows.Forms.Button DRunButton;
		private System.Windows.Forms.TextBox DSetResults;
		private System.Windows.Forms.TextBox DGetResults;
		private System.Windows.Forms.Label label8;
	}
}


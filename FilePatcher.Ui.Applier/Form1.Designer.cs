namespace FilePatcher.Ui.Applier
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
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.patchFileTextBox = new System.Windows.Forms.TextBox();
			this.targetTextBox = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.skipPreCheckCheckBox = new System.Windows.Forms.CheckBox();
			this.skipPostApplyCheck = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Patchfile:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Target Directory:";
			// 
			// patchFileTextBox
			// 
			this.patchFileTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.patchFileTextBox.Location = new System.Drawing.Point(0, 13);
			this.patchFileTextBox.Name = "patchFileTextBox";
			this.patchFileTextBox.Size = new System.Drawing.Size(284, 20);
			this.patchFileTextBox.TabIndex = 2;
			this.patchFileTextBox.DoubleClick += new System.EventHandler(this.patchFileTextBox_DoubleClick);
			// 
			// targetTextBox
			// 
			this.targetTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.targetTextBox.Location = new System.Drawing.Point(0, 46);
			this.targetTextBox.Name = "targetTextBox";
			this.targetTextBox.Size = new System.Drawing.Size(284, 20);
			this.targetTextBox.TabIndex = 3;
			this.targetTextBox.DoubleClick += new System.EventHandler(this.targetTextBox_DoubleClick);
			// 
			// button1
			// 
			this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button1.Location = new System.Drawing.Point(0, 130);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(284, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Apply Patch";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// skipPreCheckCheckBox
			// 
			this.skipPreCheckCheckBox.AutoSize = true;
			this.skipPreCheckCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.skipPreCheckCheckBox.Location = new System.Drawing.Point(0, 66);
			this.skipPreCheckCheckBox.Name = "skipPreCheckCheckBox";
			this.skipPreCheckCheckBox.Size = new System.Drawing.Size(284, 17);
			this.skipPreCheckCheckBox.TabIndex = 5;
			this.skipPreCheckCheckBox.Text = "Skip Pre Apply Check";
			this.skipPreCheckCheckBox.UseVisualStyleBackColor = true;
			// 
			// skipPostApplyCheck
			// 
			this.skipPostApplyCheck.AutoSize = true;
			this.skipPostApplyCheck.Dock = System.Windows.Forms.DockStyle.Top;
			this.skipPostApplyCheck.Location = new System.Drawing.Point(0, 83);
			this.skipPostApplyCheck.Name = "skipPostApplyCheck";
			this.skipPostApplyCheck.Size = new System.Drawing.Size(284, 17);
			this.skipPostApplyCheck.TabIndex = 6;
			this.skipPostApplyCheck.Text = "Skip Post Apply Check";
			this.skipPostApplyCheck.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button2.Location = new System.Drawing.Point(0, 107);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(284, 23);
			this.button2.TabIndex = 7;
			this.button2.Text = "Can Apply Patch?";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 153);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.skipPostApplyCheck);
			this.Controls.Add(this.skipPreCheckCheckBox);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.targetTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.patchFileTextBox);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "FilePatch Applier";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox patchFileTextBox;
		private System.Windows.Forms.TextBox targetTextBox;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox skipPreCheckCheckBox;
		private System.Windows.Forms.CheckBox skipPostApplyCheck;
		private System.Windows.Forms.Button button2;
	}
}


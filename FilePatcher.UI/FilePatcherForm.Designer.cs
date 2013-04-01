namespace FilePatcher.UI
{
	partial class FilePatcherForm
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
			this.versionHistoryPanel = new System.Windows.Forms.Panel();
			this.versionHistory = new System.Windows.Forms.ListBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.deleteButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.downButton = new System.Windows.Forms.Button();
			this.upButton = new System.Windows.Forms.Button();
			this.settingsPanel = new System.Windows.Forms.Panel();
			this.useRSycnCheckBox = new System.Windows.Forms.CheckBox();
			this.OnlyPatchToLastVersionCheckBox = new System.Windows.Forms.CheckBox();
			this.createFileDifferencesCheckBox = new System.Windows.Forms.CheckBox();
			this.patchFileNameTemplate = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.createButton = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.ignorePanel = new System.Windows.Forms.Panel();
			this.ignoreListBox = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.deleteIgnoreButton = new System.Windows.Forms.Button();
			this.addIgnoreButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.createStandAlonePatchCheckBox = new System.Windows.Forms.CheckBox();
			this.versionHistoryPanel.SuspendLayout();
			this.panel2.SuspendLayout();
			this.settingsPanel.SuspendLayout();
			this.ignorePanel.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// versionHistoryPanel
			// 
			this.versionHistoryPanel.Controls.Add(this.versionHistory);
			this.versionHistoryPanel.Controls.Add(this.panel2);
			this.versionHistoryPanel.Location = new System.Drawing.Point(3, 25);
			this.versionHistoryPanel.Name = "versionHistoryPanel";
			this.versionHistoryPanel.Size = new System.Drawing.Size(208, 232);
			this.versionHistoryPanel.TabIndex = 2;
			// 
			// versionHistory
			// 
			this.versionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.versionHistory.FormattingEnabled = true;
			this.versionHistory.HorizontalScrollbar = true;
			this.versionHistory.Location = new System.Drawing.Point(0, 0);
			this.versionHistory.Name = "versionHistory";
			this.versionHistory.Size = new System.Drawing.Size(162, 232);
			this.versionHistory.TabIndex = 2;
			this.versionHistory.SelectedIndexChanged += new System.EventHandler(this.versionHistory_SelectedIndexChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.deleteButton);
			this.panel2.Controls.Add(this.addButton);
			this.panel2.Controls.Add(this.downButton);
			this.panel2.Controls.Add(this.upButton);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(162, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(46, 232);
			this.panel2.TabIndex = 4;
			// 
			// deleteButton
			// 
			this.deleteButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.deleteButton.Enabled = false;
			this.deleteButton.Location = new System.Drawing.Point(0, 150);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(46, 50);
			this.deleteButton.TabIndex = 3;
			this.deleteButton.Text = "Delete";
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// addButton
			// 
			this.addButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.addButton.Location = new System.Drawing.Point(0, 100);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(46, 50);
			this.addButton.TabIndex = 2;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// downButton
			// 
			this.downButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.downButton.Enabled = false;
			this.downButton.Location = new System.Drawing.Point(0, 50);
			this.downButton.Name = "downButton";
			this.downButton.Size = new System.Drawing.Size(46, 50);
			this.downButton.TabIndex = 1;
			this.downButton.Text = "Down";
			this.downButton.UseVisualStyleBackColor = true;
			this.downButton.Click += new System.EventHandler(this.downButton_Click);
			// 
			// upButton
			// 
			this.upButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.upButton.Enabled = false;
			this.upButton.Location = new System.Drawing.Point(0, 0);
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size(46, 50);
			this.upButton.TabIndex = 0;
			this.upButton.Text = "Up";
			this.upButton.UseVisualStyleBackColor = true;
			this.upButton.Click += new System.EventHandler(this.upButton_Click);
			// 
			// settingsPanel
			// 
			this.settingsPanel.Controls.Add(this.createStandAlonePatchCheckBox);
			this.settingsPanel.Controls.Add(this.useRSycnCheckBox);
			this.settingsPanel.Controls.Add(this.OnlyPatchToLastVersionCheckBox);
			this.settingsPanel.Controls.Add(this.createFileDifferencesCheckBox);
			this.settingsPanel.Controls.Add(this.patchFileNameTemplate);
			this.settingsPanel.Controls.Add(this.label1);
			this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.settingsPanel.Location = new System.Drawing.Point(0, 259);
			this.settingsPanel.Name = "settingsPanel";
			this.settingsPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.settingsPanel.Size = new System.Drawing.Size(425, 105);
			this.settingsPanel.TabIndex = 3;
			// 
			// useRSycnCheckBox
			// 
			this.useRSycnCheckBox.AutoSize = true;
			this.useRSycnCheckBox.Checked = true;
			this.useRSycnCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.useRSycnCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.useRSycnCheckBox.Location = new System.Drawing.Point(0, 70);
			this.useRSycnCheckBox.Name = "useRSycnCheckBox";
			this.useRSycnCheckBox.Size = new System.Drawing.Size(425, 17);
			this.useRSycnCheckBox.TabIndex = 4;
			this.useRSycnCheckBox.Text = "Use modified RSync FileDiff (may create smaller files)";
			this.useRSycnCheckBox.UseVisualStyleBackColor = true;
			// 
			// OnlyPatchToLastVersionCheckBox
			// 
			this.OnlyPatchToLastVersionCheckBox.AutoSize = true;
			this.OnlyPatchToLastVersionCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.OnlyPatchToLastVersionCheckBox.Location = new System.Drawing.Point(0, 53);
			this.OnlyPatchToLastVersionCheckBox.Name = "OnlyPatchToLastVersionCheckBox";
			this.OnlyPatchToLastVersionCheckBox.Size = new System.Drawing.Size(425, 17);
			this.OnlyPatchToLastVersionCheckBox.TabIndex = 3;
			this.OnlyPatchToLastVersionCheckBox.Text = "Only Patch to last Version";
			this.OnlyPatchToLastVersionCheckBox.UseVisualStyleBackColor = true;
			// 
			// createFileDifferencesCheckBox
			// 
			this.createFileDifferencesCheckBox.AutoSize = true;
			this.createFileDifferencesCheckBox.Checked = true;
			this.createFileDifferencesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.createFileDifferencesCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.createFileDifferencesCheckBox.Location = new System.Drawing.Point(0, 36);
			this.createFileDifferencesCheckBox.Name = "createFileDifferencesCheckBox";
			this.createFileDifferencesCheckBox.Size = new System.Drawing.Size(425, 17);
			this.createFileDifferencesCheckBox.TabIndex = 2;
			this.createFileDifferencesCheckBox.Text = "Create File Differences";
			this.createFileDifferencesCheckBox.UseVisualStyleBackColor = true;
			// 
			// patchFileNameTemplate
			// 
			this.patchFileNameTemplate.Dock = System.Windows.Forms.DockStyle.Top;
			this.patchFileNameTemplate.Location = new System.Drawing.Point(0, 16);
			this.patchFileNameTemplate.Name = "patchFileNameTemplate";
			this.patchFileNameTemplate.Size = new System.Drawing.Size(425, 20);
			this.patchFileNameTemplate.TabIndex = 0;
			this.patchFileNameTemplate.DoubleClick += new System.EventHandler(this.patchFileNameTemplate_DoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(132, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Patch FileName Template:";
			// 
			// createButton
			// 
			this.createButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.createButton.Enabled = false;
			this.createButton.Location = new System.Drawing.Point(0, 364);
			this.createButton.Name = "createButton";
			this.createButton.Size = new System.Drawing.Size(425, 23);
			this.createButton.TabIndex = 2;
			this.createButton.Text = "Create Patch(s)";
			this.createButton.UseVisualStyleBackColor = true;
			this.createButton.Click += new System.EventHandler(this.createButton_Click);
			// 
			// ignorePanel
			// 
			this.ignorePanel.Controls.Add(this.ignoreListBox);
			this.ignorePanel.Controls.Add(this.panel3);
			this.ignorePanel.Location = new System.Drawing.Point(217, 25);
			this.ignorePanel.Name = "ignorePanel";
			this.ignorePanel.Size = new System.Drawing.Size(208, 232);
			this.ignorePanel.TabIndex = 4;
			// 
			// ignoreListBox
			// 
			this.ignoreListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ignoreListBox.FormattingEnabled = true;
			this.ignoreListBox.HorizontalScrollbar = true;
			this.ignoreListBox.Location = new System.Drawing.Point(0, 0);
			this.ignoreListBox.Name = "ignoreListBox";
			this.ignoreListBox.Size = new System.Drawing.Size(162, 232);
			this.ignoreListBox.TabIndex = 2;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.deleteIgnoreButton);
			this.panel3.Controls.Add(this.addIgnoreButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel3.Location = new System.Drawing.Point(162, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(46, 232);
			this.panel3.TabIndex = 4;
			// 
			// deleteIgnoreButton
			// 
			this.deleteIgnoreButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.deleteIgnoreButton.Enabled = false;
			this.deleteIgnoreButton.Location = new System.Drawing.Point(0, 50);
			this.deleteIgnoreButton.Name = "deleteIgnoreButton";
			this.deleteIgnoreButton.Size = new System.Drawing.Size(46, 50);
			this.deleteIgnoreButton.TabIndex = 3;
			this.deleteIgnoreButton.Text = "Delete";
			this.deleteIgnoreButton.UseVisualStyleBackColor = true;
			this.deleteIgnoreButton.Click += new System.EventHandler(this.deleteIgnoreButton_Click);
			// 
			// addIgnoreButton
			// 
			this.addIgnoreButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.addIgnoreButton.Location = new System.Drawing.Point(0, 0);
			this.addIgnoreButton.Name = "addIgnoreButton";
			this.addIgnoreButton.Size = new System.Drawing.Size(46, 50);
			this.addIgnoreButton.TabIndex = 2;
			this.addIgnoreButton.Text = "Add";
			this.addIgnoreButton.UseVisualStyleBackColor = true;
			this.addIgnoreButton.Click += new System.EventHandler(this.addIgnoreButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(0, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(133, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Versions (oldes to newest):";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(214, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Ignore list:";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Multiselect = true;
			// 
			// progressBar1
			// 
			this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar1.Location = new System.Drawing.Point(0, 387);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(425, 23);
			this.progressBar1.TabIndex = 7;
			// 
			// createStandAlonePatchCheckBox
			// 
			this.createStandAlonePatchCheckBox.AutoSize = true;
			this.createStandAlonePatchCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.createStandAlonePatchCheckBox.Location = new System.Drawing.Point(0, 87);
			this.createStandAlonePatchCheckBox.Name = "createStandAlonePatchCheckBox";
			this.createStandAlonePatchCheckBox.Size = new System.Drawing.Size(425, 17);
			this.createStandAlonePatchCheckBox.TabIndex = 5;
			this.createStandAlonePatchCheckBox.Text = "Create standalone Patch";
			this.createStandAlonePatchCheckBox.UseVisualStyleBackColor = true;
			// 
			// FilePatcherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(425, 410);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ignorePanel);
			this.Controls.Add(this.versionHistoryPanel);
			this.Controls.Add(this.settingsPanel);
			this.Controls.Add(this.createButton);
			this.Controls.Add(this.progressBar1);
			this.Name = "FilePatcherForm";
			this.Text = "FilePatcher";
			this.versionHistoryPanel.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.settingsPanel.ResumeLayout(false);
			this.settingsPanel.PerformLayout();
			this.ignorePanel.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel versionHistoryPanel;
		private System.Windows.Forms.ListBox versionHistory;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button downButton;
		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Panel settingsPanel;
		private System.Windows.Forms.TextBox patchFileNameTemplate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button createButton;
		private System.Windows.Forms.CheckBox createFileDifferencesCheckBox;
		private System.Windows.Forms.Panel ignorePanel;
		private System.Windows.Forms.ListBox ignoreListBox;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button deleteIgnoreButton;
		private System.Windows.Forms.Button addIgnoreButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBox OnlyPatchToLastVersionCheckBox;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.CheckBox useRSycnCheckBox;
		private System.Windows.Forms.CheckBox createStandAlonePatchCheckBox;

	}
}


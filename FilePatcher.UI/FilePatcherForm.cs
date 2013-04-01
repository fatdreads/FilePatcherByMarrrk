using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace FilePatcher.UI
{
	public partial class FilePatcherForm : Form
	{
		private Thread workerThread;
		private volatile bool shouldStopWorkerThread = false;

		public FilePatcherForm()
		{
			InitializeComponent();
		}

		private void patchFileNameTemplate_DoubleClick(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				patchFileNameTemplate.Text = saveFileDialog1.FileName;
			}
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				foreach (string other in versionHistory.Items)
				{
					if (folderBrowserDialog1.SelectedPath.ToLower().StartsWith(other.ToLower()))
						return;
					if (other.ToLower().StartsWith(folderBrowserDialog1.SelectedPath.ToLower()))
						return;
				}

				versionHistory.Items.Add(folderBrowserDialog1.SelectedPath);
				versionHistory.SelectedIndex = versionHistory.Items.Count - 1;
				if (versionHistory.Items.Count > 1)
					createButton.Enabled = true;
			}
		}

		private void versionHistory_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (versionHistory.SelectedIndex == -1)
			{
				deleteButton.Enabled = false;
				upButton.Enabled = false;
				downButton.Enabled = false;
			}
			else
			{
				deleteButton.Enabled = true;
				upButton.Enabled = true;
				downButton.Enabled = true;
			}
		}

		private void upButton_Click(object sender, EventArgs e)
		{
			if (versionHistory.SelectedIndex > 0)
			{
				var tmpObject = versionHistory.Items[versionHistory.SelectedIndex - 1];

				versionHistory.Items[versionHistory.SelectedIndex - 1] = versionHistory.Items[versionHistory.SelectedIndex];
				versionHistory.Items[versionHistory.SelectedIndex] = tmpObject;
			}
		}

		private void downButton_Click(object sender, EventArgs e)
		{
			if (versionHistory.SelectedIndex < versionHistory.Items.Count - 1)
			{
				var tmpObject = versionHistory.Items[versionHistory.SelectedIndex + 1];

				versionHistory.Items[versionHistory.SelectedIndex + 1] = versionHistory.Items[versionHistory.SelectedIndex];
				versionHistory.Items[versionHistory.SelectedIndex] = tmpObject;
			}
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			versionHistory.Items.RemoveAt(versionHistory.SelectedIndex);
			if (versionHistory.Items.Count <= 1)
				createButton.Enabled = false;
		}

		private void CreatePatchInBackground()
		{
			try
			{
				var fileTemplate = patchFileNameTemplate.Text;
				var fileInfo = new FileInfo(fileTemplate);
				var extension = fileInfo.Extension;
				var fileTemplatePath = fileInfo.FullName.Remove(fileInfo.FullName.Length - extension.Length, extension.Length);
				fileTemplatePath += "{0}to{1}" + extension;

				if (!OnlyPatchToLastVersionCheckBox.Checked)
				{
					for (int fromIndex = 0; fromIndex < versionHistory.Items.Count - 1; ++fromIndex)
					{
						for (int toIndex = fromIndex + 1; toIndex < versionHistory.Items.Count; ++toIndex)
						{
							if (shouldStopWorkerThread)
								return;

							var patchFilePath = string.Format(fileTemplatePath, fromIndex + 1, toIndex + 1);
							var creator = new FilePatcher.Creator((string)versionHistory.Items[fromIndex], (string)versionHistory.Items[toIndex], patchFilePath);
							creator.DontPatchFilePaths.AddRange(ignoreListBox.Items.OfType<string>());
							creator.AllowCreateFileDifferences = createFileDifferencesCheckBox.Checked;
							creator.UseRSyncFileDifferences = useRSycnCheckBox.Checked;

							creator.Create();

							if (createStandAlonePatchCheckBox.Checked)
								CreateStandAlonePatch(patchFilePath);

							this.BeginInvoke(new Action(() =>
							{
								progressBar1.Value++;
							}));
						}
					}
				}
				else
				{
					var finalPath = (string)versionHistory.Items[versionHistory.Items.Count - 1];
					for (int fromIndex = 0; fromIndex < versionHistory.Items.Count - 1; ++fromIndex)
					{
						if (shouldStopWorkerThread)
							return;

						var patchFilePath = string.Format(fileTemplatePath, fromIndex + 1, versionHistory.Items.Count);
						var creator = new FilePatcher.Creator((string)versionHistory.Items[fromIndex], finalPath, patchFilePath);
						creator.DontPatchFilePaths.AddRange(ignoreListBox.Items.OfType<string>());
						creator.AllowCreateFileDifferences = createFileDifferencesCheckBox.Checked;

						creator.Create();

						this.BeginInvoke(new Action(() =>
						{
							progressBar1.Value++;
						}));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			workerThread = null;

			this.BeginInvoke(new Action(() =>
			{
				createButton.Text = "Create Patch(s)";
				versionHistoryPanel.Enabled = true;
				settingsPanel.Enabled = true;
				ignorePanel.Enabled = true;
			}));
		}

		private void CreateStandAlonePatch(string patchFilePath)
		{
			var standalonePath = Path.Combine(Path.GetDirectoryName(patchFilePath), Path.GetFileNameWithoutExtension(patchFilePath) + ".exe");
			File.Copy("StandAlonePatcher.exe", standalonePath);
			using (var fileStream = File.OpenWrite(standalonePath))
			{
				fileStream.Position = fileStream.Length;
				using (var patchFileStream = File.OpenRead(patchFilePath))
				{
					using (var writer = new BinaryWriter(fileStream))
					{
						patchFileStream.CopyTo(fileStream);
						writer.Write(patchFileStream.Length + 8);
					}
				}
			}
		}

		private void createButton_Click(object sender, EventArgs e)
		{
			if (workerThread == null)
			{
				workerThread = new Thread(CreatePatchInBackground);

				createButton.Text = "Stop";
				versionHistoryPanel.Enabled = false;
				settingsPanel.Enabled = false;
				ignorePanel.Enabled = false;
				progressBar1.Value = 0;

				if (!OnlyPatchToLastVersionCheckBox.Checked)
				{
					progressBar1.Maximum = 0;
					for (int fromIndex = 0; fromIndex < versionHistory.Items.Count - 1; ++fromIndex)
						for (int toIndex = fromIndex + 1; toIndex < versionHistory.Items.Count; ++toIndex)
						progressBar1.Maximum++;
				}
				else
					progressBar1.Maximum = versionHistory.Items.Count - 1;

				workerThread.Start();
			}
			else
			{
				shouldStopWorkerThread = true;
				workerThread.Join();
			}
		}

		private void addIgnoreButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				ignoreListBox.Items.AddRange(openFileDialog1.FileNames);
				deleteIgnoreButton.Enabled = true;
			}
		}

		private void deleteIgnoreButton_Click(object sender, EventArgs e)
		{
			while (ignoreListBox.SelectedIndices.Count != 0)
				ignoreListBox.Items.RemoveAt(ignoreListBox.SelectedIndex);

			if (ignoreListBox.Items.Count == 0)
				deleteIgnoreButton.Enabled = false;
		}
	}
}

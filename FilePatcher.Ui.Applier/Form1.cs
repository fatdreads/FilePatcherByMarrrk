using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FilePatcher.Ui.Applier
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void patchFileTextBox_DoubleClick(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				patchFileTextBox.Text = openFileDialog1.FileName;
		}

		private void targetTextBox_DoubleClick(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				targetTextBox.Text = folderBrowserDialog1.SelectedPath;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				var applier = new FilePatcher.Applier(patchFileTextBox.Text, targetTextBox.Text);
				applier.SkipPreApplyCheck = skipPreCheckCheckBox.Checked;
				applier.SkipPostApplyCheck = skipPostApplyCheck.Checked;
				applier.Apply();
                MessageBox.Show("Done.");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				var applier = new FilePatcher.Applier(patchFileTextBox.Text, targetTextBox.Text);
				MessageBox.Show("Can Apply? " + applier.CanApply());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}

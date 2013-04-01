using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading;

namespace StandAlonePatcher
{
	public partial class PatcherForm : Form
	{
		public PatcherForm()
		{
			InitializeComponent();
		}

		private Thread workerThread;
		private string patchPath;
		private string location;

		private void PatcherForm_Shown(object sender, EventArgs e)
		{
			location = Assembly.GetEntryAssembly().Location;
			patchPath = location + ".patch";

			workerThread = new Thread(DoBackgroundWork);
			workerThread.Start();
		}

		private void DoBackgroundWork()
		{
			Exception failureException = null;
			try
			{
				CreatePatchFile();
				ApplyPatch();

			}
			catch (Exception ex)
			{
				failureException = ex;
			}
			finally
			{
				if (File.Exists(patchPath))
					File.Delete(patchPath);
			}

			this.BeginInvoke(new Action(() =>
			{
				if (failureException == null)
					this.OnPatchCompleted();
				else
					this.OnPatchFailed(failureException);
			}));
		}

		private void OnPatchCompleted()
		{
			this.Close();
		}

		private void OnPatchFailed(Exception exception)
		{
			MessageBox.Show(exception.Message);
			this.Close();
		}

		private void ApplyPatch()
		{
			var applier = new FilePatcher.Applier(patchPath, Directory.GetCurrentDirectory());
			applier.Apply();
		}

		private void CreatePatchFile()
		{
			using (var fileStream = File.OpenRead(location))
			{
				fileStream.Position = fileStream.Length - 8;
				using (var reader = new BinaryReader(fileStream, Encoding.ASCII))
				{
					var patchLength = reader.ReadInt64();
					fileStream.Position = fileStream.Length - patchLength;

					CreatePatchFile(fileStream);
				}
			}
		}

		private void CreatePatchFile(Stream stream)
		{
			if (File.Exists(patchPath))
				File.Delete(patchPath);

			using (var patchStream = File.OpenWrite(patchPath))
			{
				stream.CopyTo(patchStream);
			}
		}
	}
}

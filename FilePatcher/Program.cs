using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilePatcher
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length == 0)
				return PrintHelp();
			else if (args[0] == "create")
				return CreatePatch(args);
			else if (args[0] == "patch")
				return ApplyPatch(args);
			else
				return PrintHelp();
		}

		private static int ApplyPatch(string[] args)
		{
			if (args.Length == 1 || args.Length > 4)
				return PrintHelp();

			var patchFile = args[1];
			var directory = Directory.GetCurrentDirectory();
			string backup = null;
			if (args.Length == 3)
				directory = args[2];
			if (args.Length == 4)
				backup = args[3];

			if (!patchFile.Contains(@":\"))
				patchFile = Path.Combine(Directory.GetCurrentDirectory(), patchFile);
			if (!directory.Contains(@":\"))
				directory = Path.Combine(Directory.GetCurrentDirectory(), directory);

			var patchApplier = new Applier(patchFile, directory, backup);
			try
			{
				patchApplier.Apply();
			}
			catch (InvalidPatchVersionException ex)
			{
				Console.Error.WriteLine(ex.Message);
			}
			catch (ApplyingPatchFailedException ex)
			{
				Console.Error.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				return -1;
			}

			return 0;
		}

		private static int CreatePatch(string[] args)
		{
			if (args.Length != 4)
				return PrintHelp();

			var version1 = args[1];
			var version2 = args[2];
			var patchFile = args[3];

			if (!version1.Contains(@":\"))
				version1 = Path.Combine(Directory.GetCurrentDirectory(), version1);
			if (!version2.Contains(@":\"))
				version2 = Path.Combine(Directory.GetCurrentDirectory(), version2);
			if (!patchFile.Contains(@":\"))
				patchFile = Path.Combine(Directory.GetCurrentDirectory(), patchFile);

			var patchCreator = new Creator(version1, version2, patchFile);
			try
			{
				patchCreator.Create();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				return -1;
			}

			return 0;
		}

		private static int PrintHelp()
		{
			Console.WriteLine("FilePatcher Help:");
			Console.WriteLine("Create new patch:");
			Console.WriteLine("create <previousVersion> <currentVersion> <patchFile>");
			Console.WriteLine("Apply patch:");
			Console.WriteLine("patch <patchFile> [<targetVersion>] [<backupDirectory>]");
			return -1;
		}
	}
}

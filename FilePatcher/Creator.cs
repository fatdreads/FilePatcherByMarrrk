using System.Collections.Generic;
using System.IO;

#if !UNITY
using System.IO.Compression;
#endif

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

namespace FilePatcher
{
	public class Creator
	{
		private class FileDifference
		{
			public string FilePath;
			public long FinalLength;
			public List<FilePatcher.DifferenceExtractor.Difference> Differences = new List<FilePatcher.DifferenceExtractor.Difference>();
		}

		private string previousContentPath;
		private string currentContentPath;
		private string patchPath;

		private List<string> dontPatchFilePaths = new List<string>();

		private List<string> newDirectoryPaths;
		private List<string> oldDirectoryPaths;

		private List<FileDescriptor> previousFiles;
		private List<FileDescriptor> currentFiles;

		private List<FileDescriptor> newFileFiles;
		private List<FileDescriptor> oldFileFiles;
		private List<FileDescriptor> changedFiles;
		private List<FileDifference> changedFileDifferences = new List<FileDifference>();

		public bool AllowCreateFileDifferences = true;
		public bool UseRSyncFileDifferences = false;

		public Creator(string previousContentPath, string currentContentPath, string patchPath)
		{
			this.previousContentPath = previousContentPath.TrimEnd('\\') + "\\";
			this.currentContentPath = currentContentPath.TrimEnd('\\') + "\\";
			this.patchPath = patchPath;
		}

		public List<string> DontPatchFilePaths
		{
			get { return dontPatchFilePaths; }
		}

		public void Create()
		{
			CollectFileStates();
			CollectDifferences();
			WritePatchFile();
		}

		private void WritePatchFile()
		{
			using (var compressedFileStream = File.Create(patchPath))
			{
				Stream usableStream = compressedFileStream;
#if !UNITY
				using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
				{
					usableStream = compressionStream;
#endif
					using (var writer = new BinaryWriter(usableStream, Encoding.ASCII))
					{
						WriteFileDescriptors(writer, previousFiles);

						WriteOldDirectories(writer);
						WriteNewDirectories(writer);

						WriteOldFiles(writer);
						WriteNewFiles(writer);

						WriteChangedFiles(writer);

						WriteFileDescriptors(writer, currentFiles);
					}
#if !UNITY
				}
#endif
			}
		}

		private void WriteFileDescriptors(BinaryWriter writer, List<FileDescriptor> fileDescriptors)
		{
			writer.Write(fileDescriptors.Count);
			foreach (var fileDescriptor in fileDescriptors)
			{
				writer.Write(fileDescriptor.FilePath);
				writer.Write(fileDescriptor.Hash.Length);
				writer.Write(fileDescriptor.Hash);
			}
		}

		private void WriteChangedFiles(BinaryWriter writer)
		{
			writer.Write(changedFiles.Count);
			foreach (var fileDescriptor in changedFiles)
			{
				writer.Write(fileDescriptor.FilePath);
				var currentPath = Path.Combine(currentContentPath, fileDescriptor.FilePath);
				var fileInfo = new FileInfo(currentPath);
				writer.Write(fileInfo.Length);
			}

			foreach (var fileDifference in changedFileDifferences)
			{
				var currentPath = Path.Combine(currentContentPath, fileDifference.FilePath);

				writer.Write(fileDifference.Differences.Count);
				writer.Write(UseRSyncFileDifferences);

				using (var fileStream = File.OpenRead(currentPath))
				{
					foreach (var difference in fileDifference.Differences)
					{
						writer.Write(difference.Start);
						writer.Write(difference.Length);
						writer.Write(difference.CopyStartFromTarget);

						if (difference.CopyStartFromTarget == -1)
						{
							fileStream.Position = difference.Start;
							for (int i = 0; i < difference.Length; ++i)
							{
								var data = (byte)fileStream.ReadByte();
								writer.Write(data);
							}
						}
					}
				}
			}
		}

		private void WriteNewFiles(BinaryWriter writer)
		{
			writer.Write(newFileFiles.Count);
			foreach (var fileDescriptor in newFileFiles)
			{
				writer.Write(fileDescriptor.FilePath);
				var currentPath = Path.Combine(currentContentPath, fileDescriptor.FilePath);
				var fileInfo = new FileInfo(currentPath);
				writer.Write(fileInfo.Length);
			}

			writer.Flush();
			foreach (var fileDescriptor in newFileFiles)
			{
				var currentPath = Path.Combine(currentContentPath, fileDescriptor.FilePath);
				using (var fileStream = File.OpenRead(currentPath))
					fileStream.CopyTo(writer.BaseStream);
			}
			writer.Flush();
		}

		private void WriteOldFiles(BinaryWriter writer)
		{
			writer.Write(oldFileFiles.Count);
			foreach (var fileDescriptor in oldFileFiles)
				writer.Write(fileDescriptor.FilePath);
		}

		private void WriteNewDirectories(BinaryWriter writer)
		{
			writer.Write(newDirectoryPaths.Count);
			foreach (var filePath in newDirectoryPaths)
				writer.Write(filePath);
		}

		private void WriteOldDirectories(BinaryWriter writer)
		{
			writer.Write(oldDirectoryPaths.Count);
			foreach (var filePath in oldDirectoryPaths)
				writer.Write(filePath);
		}

		private void CollectDifferences()
		{
			foreach (var changedFile in changedFiles)
				CollectDifferences(changedFile.FilePath);
		}

		private void CollectDifferences(string filePath)
		{
			var fileDifference = new FileDifference()
			{
				FilePath = filePath
			};

			var previousPath = Path.Combine(previousContentPath, filePath);
			var currentPath = Path.Combine(currentContentPath, filePath);

			using (var previousFileStream = File.OpenRead(previousPath))
			using (var previousStream = new BinaryReader(previousFileStream, Encoding.ASCII))
			{
				using (var currentFileStream = File.OpenRead(currentPath))
				using (var currentStream = new BinaryReader(currentFileStream, Encoding.ASCII))
				{
					fileDifference.FinalLength = currentStream.BaseStream.Length;

					DifferenceExtractor.IStreamDifferenceExtractor diffExtractor;
					if (UseRSyncFileDifferences)
						diffExtractor = new DifferenceExtractor.RSync();
					else
						diffExtractor = new DifferenceExtractor.Simple();

					fileDifference.Differences = diffExtractor.GatherDifferences(previousFileStream, currentFileStream);

				}
			}

			changedFileDifferences.Add(fileDifference);
		}

		private void CollectFileStates()
		{
			var previousDirectoriePaths = Directory.EnumerateDirectories(previousContentPath, "*", SearchOption.AllDirectories)
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())))
				.Select(path => path.Remove(0, previousContentPath.Length))
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())));
			var previousFilePaths = Directory.EnumerateFiles(previousContentPath, "*", SearchOption.AllDirectories)
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())))
				.Select(path => path.Remove(0, previousContentPath.Length))
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())));
			
			var currentDirectoriePaths = Directory.EnumerateDirectories(currentContentPath, "*", SearchOption.AllDirectories)
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())))
				.Select(path => path.Remove(0, currentContentPath.Length))
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())));
			var currentFilePaths = Directory.EnumerateFiles(currentContentPath, "*", SearchOption.AllDirectories)
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())))
				.Select(path => path.Remove(0, currentContentPath.Length))
				.Where(path => DontPatchFilePaths.All(dontPatch => !path.ToLower().StartsWith(dontPatch.ToLower())));

			newDirectoryPaths = currentDirectoriePaths.Except(previousDirectoriePaths).ToList();
			newFileFiles = currentFilePaths.Except(previousFilePaths).Select(path => GenerateFileDescriptor(path, false, false)).ToList();

			previousFiles = previousFilePaths.Select(path => GenerateFileDescriptor(path, true, false)).ToList();
			currentFiles = currentFilePaths.Select(path => GenerateFileDescriptor(path, false, true)).ToList();

			oldDirectoryPaths = previousDirectoriePaths.Except(currentDirectoriePaths).ToList();
			oldFileFiles = previousFilePaths.Except(currentFilePaths).Select(path => GenerateFileDescriptor(path, false, false)).ToList();
			
			changedFiles = previousFilePaths.Intersect(currentFilePaths)
					.Where(filePath => HasFileDifference(filePath)).Select(path => GenerateFileDescriptor(path, false, false)).ToList();

			if (!AllowCreateFileDifferences)
			{
                oldFileFiles.AddRange(changedFiles);
                newFileFiles.AddRange(changedFiles);
                
                changedFiles.Clear();
			}
		}

		private FileDescriptor GenerateFileDescriptor(string path, bool generatePreviousHash, bool generateCurrentHash)
		{
			byte[] hash = new byte[0];
			
			var md5 = new MD5CryptoServiceProvider();

			var currentPath = Path.Combine(currentContentPath, path);
			var previousPath = Path.Combine(previousContentPath, path);

			if (generateCurrentHash && File.Exists(currentPath))
				using (var fileStream = File.OpenRead(currentPath))
					hash = md5.ComputeHash(fileStream);

			if (generatePreviousHash && File.Exists(previousPath))
				using (var fileStream = File.OpenRead(previousPath))
					hash = md5.ComputeHash(fileStream);
			
			return new FileDescriptor
			{
				FilePath = path,
				Hash = hash,
			};
		}

		private bool HasFileDifference(string filePath)
		{
			var previousPath = Path.Combine(previousContentPath, filePath);
			var currentPath = Path.Combine(currentContentPath, filePath);

			using (var previousFileStream = File.OpenRead(previousPath))
			{
				using (var currentFileStream = File.OpenRead(currentPath))
				{
					if (previousFileStream.Length != currentFileStream.Length)
						return true;

                    //var hash = new MD5CryptoServiceProvider();
                    var hash = HashAlgorithm.Create();

					var fileHash1 = hash.ComputeHash(previousFileStream);
					var fileHash2 = hash.ComputeHash(currentFileStream);

					return BitConverter.ToString(fileHash1) != BitConverter.ToString(fileHash2);
				}
			}
		}

	}
}

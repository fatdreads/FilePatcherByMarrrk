using System.Collections.Generic;
using System.IO;

#if !UNITY
using System.IO.Compression;
#endif

using System.Security.Cryptography;
using System.Linq;
using System;
using System.Text;

namespace FilePatcher
{
	public class InvalidPatchVersionException : Exception
	{
		public InvalidPatchVersionException(string filePath)
			: base("Unable to patch, invalid file hash: " + filePath)
		{
		}
	}

	public class ApplyingPatchFailedException : Exception
	{
		public ApplyingPatchFailedException(string filePath)
			: base("Patching failed, invalid resulting file hash: " + filePath)
		{
		}
	}

	public class Applier
	{
		private string targetPath;
		private string patchPath;

		public bool SkipPreApplyCheck = false;
		public bool SkipPostApplyCheck = false;

		public Applier(string patchPath, string targetPath)
			: this(patchPath, targetPath, null)
		{
		}

		public Applier(string patchPath, string targetPath, string backupPath)
		{
			this.patchPath = patchPath;
			this.targetPath = targetPath.TrimEnd('\\') + "\\";

			if (!string.IsNullOrEmpty(backupPath))
			{
				if (Directory.Exists(backupPath))
					Directory.Delete(backupPath, true);
				DirectoryCopy(this.targetPath, backupPath, true);
			}
		}

		public bool CanApply()
		{
			using (var compressedFileStream = File.OpenRead(patchPath))
			{
				Stream usableStream = compressedFileStream;
#if !UNITY
				using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
				{
					usableStream = compressionStream;
#endif
					using (var reader = new BinaryReader(usableStream, Encoding.ASCII))
					{
						try
						{
							VerifyFileDescriptors(reader, true);
						}
						catch (InvalidPatchVersionException)
						{
							return false;
						}
					}
#if !UNITY
				}
#endif
			}
			return true;
		}

		public void Apply()
		{
			using (var compressedFileStream = File.OpenRead(patchPath))
			{
				Stream usableStream = compressedFileStream;
#if !UNITY
				using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
				{
					usableStream = compressionStream;
#endif
					using (var reader = new BinaryReader(usableStream, Encoding.ASCII))
					{
						VerifyFileDescriptors(reader, true);

						RemoveOldDirectories(reader);
						CreateNewDirectories(reader);

						RemoveOldFiles(reader);
						CreateNewFiles(reader);

						PatchChangedFiles(reader);

						VerifyFileDescriptors(reader, false);
					}
#if !UNITY
				}
#endif
			}
		}

		private void VerifyFileDescriptors(BinaryReader reader, bool preTest)
		{
			var count = reader.ReadInt32();
			for (int i = 0; i < count; ++i)
			{
				var filePath = reader.ReadString();
				var hashByteCount = reader.ReadInt32();
				var hash = reader.ReadBytes(hashByteCount);

				if ((preTest && !SkipPreApplyCheck) ||
					(!preTest && !SkipPostApplyCheck))
					CheckFileHash(filePath, hash, preTest);
			}
		}

		private void CheckFileHash(string filePath, byte[] hash, bool preTest)
		{
			var currentPath = Path.Combine(targetPath, filePath);
			if (!File.Exists(currentPath))
			{
				if (preTest)
					throw new InvalidPatchVersionException(filePath);
				else
					throw new ApplyingPatchFailedException(filePath);
			}

			var md5 = new MD5CryptoServiceProvider();
			byte[] otherHash = null;
			using (var fileStream = File.OpenRead(currentPath))
				otherHash = md5.ComputeHash(fileStream);

			if (otherHash.Length != hash.Length || !otherHash.SequenceEqual(hash))
			{
				if (preTest)
					throw new InvalidPatchVersionException(filePath);
				else
					throw new ApplyingPatchFailedException(filePath);
			}
		}

		private void PatchChangedFiles(BinaryReader reader)
		{
			var count = reader.ReadInt32();
			var fileInfos = new List<KeyValuePair<string, long>>(count);

			for (int i = 0; i < count; ++i)
			{
				var filePath = reader.ReadString();
				var currentPath = Path.Combine(targetPath, filePath);
				var length = reader.ReadInt64();

				fileInfos.Add(new KeyValuePair<string, long>(currentPath, length));
			}

			foreach (var fileInfo in fileInfos)
			{
				var filePath = fileInfo.Key;
				var length = fileInfo.Value;

				count = reader.ReadInt32();
				var useRSync = reader.ReadBoolean();

				if (useRSync)
					PatchFileRSync(reader, count, filePath, length);
				else
				{
					PatchFileSimple(reader, count, filePath, length);
				}
			}
		}

		private static void PatchFileSimple(BinaryReader reader, int countDifferences, string filePath, long finalFileLength)
		{
			using (var fileStream = File.OpenWrite(filePath))
			{
				fileStream.SetLength(finalFileLength);
				for (int i = 0; i < countDifferences; ++i)
				{
					var differenceStart = reader.ReadInt64();
					var differenceLength = reader.ReadInt64();
					var differenceCopyStartFromTarget = reader.ReadInt64();
					if (differenceCopyStartFromTarget >= 0)
						throw new InvalidDataException("Patch has RSync data but this was not expected.");

					fileStream.Position = differenceStart;
					for (int k = 0; k < differenceLength; ++k)
					{
						var data = reader.ReadByte();
						fileStream.WriteByte(data);
					}
				}
			}
		}

		private void PatchFileRSync(BinaryReader reader, int countDifferences, string filePath, long finalFileLength)
		{
			var tmpFileName = Path.Combine(targetPath, Guid.NewGuid().ToString() + ".tmp");
			try
			{
				File.Copy(filePath, tmpFileName);
				using (var tmpFileStream = File.OpenWrite(tmpFileName))
				{
					using (var fileStream = File.OpenRead(filePath))
					{
						using (var fileStreamReader = new BinaryReader(fileStream, Encoding.ASCII))
						{
							tmpFileStream.SetLength(finalFileLength);
							for (int i = 0; i < countDifferences; ++i)
							{
								var differenceStart = reader.ReadInt64();
								var differenceLength = reader.ReadInt64();
								var differenceCopyStartFromTarget = reader.ReadInt64();

								tmpFileStream.Position = differenceStart;
								if (differenceCopyStartFromTarget >= 0)
								{
									fileStream.Position = differenceCopyStartFromTarget;
									for (int k = 0; k < differenceLength; ++k)
									{
										var data = fileStreamReader.ReadByte();
										tmpFileStream.WriteByte(data);
									}
								}
								else
								{
									for (int k = 0; k < differenceLength; ++k)
									{
										var data = reader.ReadByte();
										tmpFileStream.WriteByte(data);
									}
								}
							}
						}
					}
				}

				File.Delete(filePath);
				File.Move(tmpFileName, filePath);
			}
			finally
			{
				if (File.Exists(tmpFileName))
					File.Delete(tmpFileName);
			}
		}

		private void CreateNewFiles(BinaryReader reader)
		{
			var count = reader.ReadInt32();
			var fileInfos = new List<KeyValuePair<string, long>>(count);

			for (int i = 0; i < count; ++i)
			{
				var filePath = reader.ReadString();
				var currentPath = Path.Combine(targetPath, filePath);
				var length = reader.ReadInt64();

				fileInfos.Add(new KeyValuePair<string, long>(currentPath, length));
			}

			foreach (var fileInfo in fileInfos)
			{
				var filePath = fileInfo.Key;
				var length = fileInfo.Value;
				if (File.Exists(filePath))
					File.Delete(filePath);

				using (var targetFileStream = File.Create(filePath))
				{
					for (long i = 0; i < length; ++i)
						targetFileStream.WriteByte(reader.ReadByte());
				}
			}
		}

		private void RemoveOldFiles(BinaryReader reader)
		{
			var count = reader.ReadInt32();
			for (int i = 0; i < count; ++i)
			{
				var filePath = reader.ReadString();
				filePath = Path.Combine(targetPath, filePath);
				if (File.Exists(filePath))
					File.Delete(filePath);
			}
		}

		private void RemoveOldDirectories(BinaryReader reader)
		{
			var count = reader.ReadInt32();
			for (int i = 0; i < count; ++i)
			{
				var path = reader.ReadString();
				path = Path.Combine(targetPath, path);
				if (Directory.Exists(path))
					Directory.Delete(path, true);
			}
		}

		private void CreateNewDirectories(BinaryReader reader)
		{
			var count = reader.ReadInt32();
			for (int i = 0; i < count; ++i)
			{
				var path = reader.ReadString();
				path = Path.Combine(targetPath, path);
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
			}
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	}
}

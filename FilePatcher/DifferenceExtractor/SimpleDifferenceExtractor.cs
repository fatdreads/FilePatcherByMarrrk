using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilePatcher.DifferenceExtractor
{
	public class Simple : IStreamDifferenceExtractor
	{
		#region IStreamDifferenceExtractor Members

		public List<Difference> GatherDifferences(Stream streamA, Stream streamB)
		{
			streamA.Position = 0;
			streamB.Position = 0;

			var differences = new List<Difference>();
			Difference currentDifference = null;
			long currentPosition = 0;

			var previousStream = new BinaryReader(streamA, Encoding.ASCII);
			var currentStream = new BinaryReader(streamB, Encoding.ASCII);
			while (previousStream.PeekChar() != -1 && currentStream.PeekChar() != -1)
			{
				CheckFileDifference(differences, ref currentDifference, previousStream, currentStream, currentPosition);
				currentPosition++;
			}
			while (currentStream.PeekChar() != -1)
			{
				AddFileOverhang(differences, ref currentDifference, previousStream, currentStream, currentPosition);
				currentPosition++;
			}

			return differences;
		}

		private void AddFileOverhang(List<Difference> differences, ref Difference currentDifference, BinaryReader previousStream, BinaryReader currentStream, long currentPosition)
		{
			currentPosition++;
			var currentByte = (byte)currentStream.Read();
			if (currentDifference == null)
			{
				currentDifference = new Difference()
				{
					Start = currentPosition
				};
				differences.Add(currentDifference);
			}
			currentDifference.Length++;
		}

		private void CheckFileDifference(List<Difference> differences, ref Difference currentDifference, BinaryReader previousStream, BinaryReader currentStream, long currentPosition)
		{
			var currentPosition2 = currentStream.BaseStream.Position;
			var currentByte = (byte)currentStream.ReadByte();
			if (previousStream.ReadByte() != currentByte)
			{
				if (currentDifference == null)
				{
					currentDifference = new Difference()
					{
						Start = currentPosition
					};
					differences.Add(currentDifference);
				}
				currentDifference.Length++;
			}
			else
				currentDifference = null;
		}


		#endregion
	}
}

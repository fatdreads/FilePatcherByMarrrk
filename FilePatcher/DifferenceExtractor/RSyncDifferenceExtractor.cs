using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilePatcher.DifferenceExtractor
{
	public class RSync : IStreamDifferenceExtractor
	{
		private class Block
		{
			public long Start;
			public long Length;
			public long Weak;
			public byte[] Strong;
		}

		#region IStreamDifferenceExtractor Members

		public int BlockSize = 1024;

		public List<Difference> GatherDifferences(Stream streamA, Stream streamB)
		{
			streamA.Position = 0;
			streamB.Position = 0;

			var previousStream = new BinaryReader(streamA, Encoding.ASCII);
			var otherStream = new BinaryReader(streamB, Encoding.ASCII);

			var factoryData = previousStream.ReadBytes((int)previousStream.BaseStream.Length);

			var blocks = new List<Block>();
			while (otherStream.PeekChar() >= 0)
			{
				var start = otherStream.BaseStream.Position;
				var blockSize = Math.Min(BlockSize, (int)(streamB.Length - streamB.Position));

				var data = otherStream.ReadBytes(blockSize);
				var weak = RollingChecksum.Weak(data, 0, data.Length);
				var strong = RollingChecksum.Strong(data, 0, data.Length);

				var block = new Block()
				{
					Start = start,
					Length = blockSize,
					Strong = strong,
					Weak = weak
				};
				blocks.Add(block);
			}

			var differences = GenerateDifferences(blocks, factoryData);
			return differences;
		}

		private List<Difference> GenerateDifferences(List<Block> blocks, byte[] factoryData)
		{
			var differences = new List<Difference>();
			var weakMap = new Dictionary<long, List<long>>();

			for (int i = 0; i < factoryData.Length; ++i )
			{
				var blockSize = Math.Min(BlockSize, factoryData.Length - i);
				var weak = RollingChecksum.Weak(factoryData, i, blockSize);

				List<long> indices;
				if (!weakMap.TryGetValue(weak, out indices))
				{
					indices = new List<long>();
					weakMap.Add(weak, indices);
				}
				indices.Add(i);
				if (i + 1 >= factoryData.Length - BlockSize)
					break;
			}

			for (int i = 0; i < blocks.Count; )
			{
				var block = blocks[i];
				List<long> indices;
				if (!weakMap.TryGetValue(block.Weak, out indices))
				{
					++i;
					continue;
				}

				var found = false;
				foreach (var index in indices)
				{
					if ((factoryData.Length - index) < block.Length)
						continue;

					var strong = RollingChecksum.Strong(factoryData, (int)index, (int)block.Length);

					if (strong.SequenceEqual(block.Strong))
					{
						found = true;
						blocks.RemoveAt(i);

						if (index == block.Start)
							break;

						var difference = new Difference()
						{
							CopyStartFromTarget = index,
							Start = block.Start,
							Length = block.Length
						};
						differences.Add(difference);

                        break;
					}
				}

				if (!found)
					++i;
			}

			foreach (var block in blocks)
			{
				var difference = new Difference()
				{
					Start = block.Start,
					Length = block.Length
				};
				differences.Add(difference);
			}

			return differences;
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FilePatcher.DifferenceExtractor
{
	public class Difference
	{
		public long Start;
		public long Length;
		public long CopyStartFromTarget = -1;
	}

	public interface IStreamDifferenceExtractor
	{
		List<Difference> GatherDifferences(Stream streamA, Stream streamB);
	}
}

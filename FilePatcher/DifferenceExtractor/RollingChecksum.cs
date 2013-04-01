using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FilePatcher.DifferenceExtractor
{
	public static class RollingChecksum
	{
        private static double M = Math.Pow(2, 16);
       
        public static long Weak(byte[] data, int fromIndex, int length)
		{
            var a = 0L;
			for (int i = fromIndex; i < length; ++i)
                a += data[i];

            a = (long) (a % M);
               
            var b = 0L;
			for (int i = fromIndex; i < length; ++i)
				b += (length - 1 - i + 1) * data[i];
            b = (long) (b % M);
               
            return a + (long) (M * b);
        }

		public static byte[] Strong(byte[] data, int fromIndex, int length)
		{
			var md5 = new MD5CryptoServiceProvider();
			return md5.ComputeHash(data, fromIndex, length);
        }
	}
}

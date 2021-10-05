using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointZerver.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Removes all characters starting from the first null character in the sequence.
        /// </summary>
        /// <param name="bytes">This byte array.</param>
        /// <returns></returns>
        public static Task<byte[]> CopyRemovingNulls(this byte[] bytes)
        {
            List<byte> byteList = new(200);
            byte count = 0;
            while (bytes[count] != 0) byteList.Add(bytes[count++]);
           return Task.FromResult(byteList.ToArray());
        }
    }
}
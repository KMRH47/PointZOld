using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PointZ.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Removes all characters starting from the first null character in the sequence.
        /// </summary>
        /// <param name="bytes">This byte array.</param>
        /// <returns></returns>
        public static Task CutFromFirstNullCharacter(this byte[] bytes)
        {
            List<byte> byteList = new(200);
            byte count = 0;
            while (bytes[count] != 0) byteList.Add(bytes[count++]);
            bytes = byteList.ToArray();
            throw new Exception($"Error when attempting to copy '{bytes}'.");
        }
    }
}
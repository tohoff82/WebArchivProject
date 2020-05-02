using System;
using System.Security.Cryptography;
using System.Text;

using WebArchivProject.Contracts;

namespace WebArchivProject.Services
{
    class ServCryptografy : IServCryptografy
    {
        private const int _idSize = 5;
        private static readonly char[] _chars
            = "abcdefghijkmnpqrstuvwxyz"
                .ToCharArray();

        public string AuthorsRowId => GenerateCodeStr(_idSize);

        private string GenerateCodeStr(int size)
        {
            byte[] data = new byte[4 * size];

            using var crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(data);

            var sb = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                uint rnd = BitConverter.ToUInt32(data, i * 4);
                long idx = rnd % _chars.Length;

                sb.Append(_chars[idx]);
            }

            return sb.ToString();
        }
    }
}

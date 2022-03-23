using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TAPPT.Web.Helpers
{
    public class RandomCodeGenerator
    {
        public static string GeneratePassword() => $"{RandomCode(8)}";
        static string RandomCode(int length)
        {
            var alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numbers = "1234567890";
            var allowedChars = alphabets + numbers;
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("Length", "Length cannot be zero");
            }
            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentException("AllowedChars cannot be empty");
            }
            var byteSize = 256;
            var hash = new HashSet<char>(allowedChars);
            var allowedCharSet = hash.ToArray();
            if (byteSize < allowedCharSet.Length)
            {
                throw new ArgumentException($"AllowedChars cannot contain more than {byteSize}");
            }
            var rng = new RNGCryptoServiceProvider();
            var result = new StringBuilder();
            var buff = new byte[128];
            while (result.Length < length)
            {
                rng.GetNonZeroBytes(buff);
                for (int i = 0; i <= buff.Length - 1; i++)
                {
                    if (result.Length >= length)
                    {
                        break;
                    }
                    var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                    if (outOfRangeStart <= buff[i])
                    {
                        continue;
                    }
                    result.Append(allowedCharSet[buff[i] % allowedCharSet.Length]);
                }
            }
            return result.ToString();
        }
    }
}

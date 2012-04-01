using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SWTOR.Parser
{
    public class MD5HashCreator : IHashCreator
    {
        public string CreateHash(string data)
        {
            var MD5 = new MD5CryptoServiceProvider();
            char[] bArr = data.ToCharArray();
            int size = bArr.GetUpperBound(0);
            byte[] arr = new byte[size];
            System.Text.Encoding.Default.GetEncoder().GetBytes(bArr, 0, size, arr, 0, true);
            var retVal = MD5.ComputeHash(arr);

            var sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

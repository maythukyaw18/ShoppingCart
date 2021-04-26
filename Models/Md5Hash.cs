using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class Md5Hash
    {
        public static string GetMd5Hash(MD5 md5Hash, string str)
        {
            byte[] b = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            foreach(byte s in b)
            {
                sBuilder.Append(s.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}

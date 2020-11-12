using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BLL
{
    public static class Utils
    {
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static byte[] ConvertBytesToImage(string path)
        {
            if (path != null)
            {
                Bitmap bmp = null;
                if (File.Exists(path))
                {
                    bmp = new Bitmap(path);
                    ImageConverter converter = new ImageConverter();
                    return (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                }
            }
            return null;
        }

    }
}

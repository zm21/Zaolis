using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BLL.Models
{
    [DataContract]
    [Serializable]

    public partial class UserDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string Bio { get; set; }

        [DataMember]
        public int AvatarId { get; set; }

        [DataMember]
        public DateTime LastActive { get; set; }

        [DataMember]
        public byte[] AvatarBytes;

        public BitmapSource AvatarImage()
        {
            Bitmap bmp = null;
            using (var ms = new MemoryStream(AvatarBytes))
            {
                bmp = new Bitmap(ms);
            }
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}

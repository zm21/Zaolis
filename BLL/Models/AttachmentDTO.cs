using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BLL.Models
{
    [DataContract]
    public class AttachmentDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int MessageId { get; set; }
        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public byte[] imageBytes;

        public BitmapSource CurrentImage
        {
            get
            {
                return GetImage();
            }
        }


        public BitmapSource GetImage()
        {
            if (imageBytes?.Length > 0)
            {
                Bitmap bmp = null;
                using (var ms = new MemoryStream(imageBytes))
                {
                    bmp = new Bitmap(ms);
                }
                var handle = bmp.GetHbitmap();
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            return null;
        }
    }
}

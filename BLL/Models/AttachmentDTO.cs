using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    [ServiceContract]
    public class AttachmentDTO
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Path { get; set; }
    }
}

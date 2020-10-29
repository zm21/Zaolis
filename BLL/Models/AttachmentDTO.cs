using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
    }
}

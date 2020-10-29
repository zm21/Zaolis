using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    [DataContract]
    public class MessageDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; } //Sender id
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public DateTime CreationTime { get; set; }
    }
}

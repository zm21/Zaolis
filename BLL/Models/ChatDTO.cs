using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    [DataContract]
    public class ChatDTO
    {
        [DataMember]
        public int Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    [DataContract]
    public class VerificationCodeDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public DateTime CreationTime { get; set; }
        [DataMember]
        public int UserId { get; set; }
    }
}

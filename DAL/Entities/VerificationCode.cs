using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class VerificationCode
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DateTime CreationTime { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}

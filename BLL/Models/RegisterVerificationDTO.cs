using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RegisterVerificationDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
    }
}

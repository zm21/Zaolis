using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public partial class UserDTO
    {
        public int Id { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Bio { get; set; }
        public int AvatarId { get; set; }
    }
}

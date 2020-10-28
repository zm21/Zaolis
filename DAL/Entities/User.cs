using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        public User()
        {
            this.Chats = new HashSet<Chat>();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int AvatarId { get; set; }
        public int UserContactId { get; set; }
        public virtual Avatar Avatar { get; set; }
        public virtual UserContact UserContact { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}

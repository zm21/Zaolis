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
            this.Avatars = new HashSet<Avatar>();
            this.Messages = new HashSet<Message>();
            this.VerificationCodes = new HashSet<VerificationCode>();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Avatar> Avatars { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<VerificationCode> VerificationCodes { get; set; }
        public virtual UserContact UserContact { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}

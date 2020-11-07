using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Chat
    {
        public Chat()
        {
            this.Messages = new HashSet<Message>();
            this.Users = new HashSet<User>();
        }
        public int Id { get; set; }
        public Message LastMessage { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Message
    {
        public Message()
        {
            this.Attachments = new HashSet<Attachment>();
        }
        public int Id { get; set; }
        public int UserId { get; set; } //Sender id
        public int ChatId { get; set; }
        public string MessageText { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual User User { get; set; }
        public virtual Chat Chat { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}

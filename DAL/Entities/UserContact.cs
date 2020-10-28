using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserContact
    {
        public UserContact()
        {
            this.Contacts = new HashSet<User>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<User> Contacts { get; set; }
        public virtual User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Avatar
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}

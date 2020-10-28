using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    class UserContactConfig : EntityTypeConfiguration<UserContact>
    {
        public UserContactConfig()
        {
            this.HasKey(u => u.Id);
            this.HasMany(u => u.Contacts).WithRequired(c => c.UserContact).HasForeignKey(i => i.UserContactId);
        }
    }
}

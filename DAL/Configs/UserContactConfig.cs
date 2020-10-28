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
            this.HasKey(uc => uc.UserId);
            this.HasRequired(uc => uc.User).WithRequiredDependent(u => u.UserContact);
            this.HasMany(uc => uc.Contacts);
        }
    }
}

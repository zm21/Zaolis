using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    class AvatarConfig : EntityTypeConfiguration<Avatar>
    {
        public AvatarConfig()
        {
            this.HasKey(a => a.Id);
            this.HasRequired(a => a.User).WithMany(u => u.Avatars).HasForeignKey(a => a.UserId);
        }
    }
}

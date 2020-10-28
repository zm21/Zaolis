using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            this.HasKey(u => u.Id);
            this.Property(u => u.Login).IsRequired().HasMaxLength(64);
            this.Property(u => u.Password).IsRequired().HasMaxLength(128);
            this.Property(u => u.Bio).IsOptional().HasMaxLength(512);
        }
    }
}

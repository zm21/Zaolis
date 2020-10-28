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
            this.Property(u => u.Bio).HasMaxLength(70);
            this.Property(u => u.Email).IsRequired().HasMaxLength(70);
            this.Property(u => u.IsActive).IsRequired();
            this.Property(u => u.Login).IsRequired().HasMaxLength(70);
            this.Property(u => u.Name).IsRequired().HasMaxLength(70);
            this.Property(u => u.PasswordHash).IsRequired();
        }
    }
}

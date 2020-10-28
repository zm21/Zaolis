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
            this.HasKey(u => u.Id);
            this.Property(u => u.Path).IsOptional();
        }
    }
}

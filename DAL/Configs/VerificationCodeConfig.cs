using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    class VerificationCodeConfig : EntityTypeConfiguration<VerificationCode>
    {
        public VerificationCodeConfig()
        {
            this.HasKey(a => a.Id);
            this.HasRequired(a => a.User).WithMany(u => u.VerificationCodes).HasForeignKey(a => a.UserId);
            this.Property(a => a.Code).IsRequired().HasMaxLength(999999);
            this.Property(a => a.CreationTime).IsRequired();
        }
    }
}

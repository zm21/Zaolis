using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    public class RegisterVerificationConfig : EntityTypeConfiguration<RegisterVerification>
    {
        public RegisterVerificationConfig()
        {
            this.HasKey(u => u.Id);
            this.HasRequired(u => u.Email);
        }
    }
}

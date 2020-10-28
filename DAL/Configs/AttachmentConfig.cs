using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    class AttachmentConfig : EntityTypeConfiguration<Attachment>
    {
        public AttachmentConfig()
        {
            this.HasKey(a => a.Id);
            this.HasRequired(a => a.Message).WithMany(m => m.Attachments).HasForeignKey(a => a.MessageId);
        }
    }
}

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
            this.HasKey(u => u.Id);
            this.Property(u => u.Path).IsRequired();
            this.HasRequired(u => u.Message).WithMany(i=>i.Attachments).HasForeignKey(c=>c.MessageId);
        }
    }
}

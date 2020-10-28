using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    class MessageConfig : EntityTypeConfiguration<Message>
    {
        public MessageConfig()
        {
            this.HasKey(u => u.Id);
            this.HasMany(a => a.Attachments).WithRequired(c => c.Message).HasForeignKey(i => i.MessageId);
        }
    }
}

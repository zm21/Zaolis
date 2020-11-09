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
            this.HasKey(m => m.Id);
            this.HasRequired(m => m.User).WithMany(u => u.Messages).HasForeignKey(m => m.UserId);
            this.HasRequired(m => m.Chat).WithMany(c => c.Messages).HasForeignKey(m => m.ChatId);
            this.Property(m => m.CreationTime).IsRequired();
            this.Property(m => m.MessageText).IsRequired().HasMaxLength(1024);
        }
    }
}

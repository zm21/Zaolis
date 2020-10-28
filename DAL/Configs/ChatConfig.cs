using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configs
{
    public class ChatConfig : EntityTypeConfiguration<Chat>
    {
        public ChatConfig()
        {
            this.HasKey(u => u.Id);
            this.HasMany(u => u.Messages).WithRequired(c=>c.Chat).HasForeignKey(i=>i.ChatId);
            this.HasMany(u => u.Users).WithMany(c => c.Chats);
        }
    }
}

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
            this.HasKey(c => c.Id);
            this.HasMany(c => c.Users).WithMany(u => u.Chats);
        }
    }
}

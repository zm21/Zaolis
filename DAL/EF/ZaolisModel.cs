namespace DAL
{
    using DAL.Configs;
    using DAL.EF;
    using DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ZaolisModel : DbContext
    {
        public ZaolisModel()
            : base("name=zaolis")
        {
            Database.SetInitializer(new Initializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AttachmentConfig());
            modelBuilder.Configurations.Add(new AvatarConfig());
            modelBuilder.Configurations.Add(new ChatConfig());
            modelBuilder.Configurations.Add(new MessageConfig());
            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new UserContactConfig());
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Avatar> Avatars { get; set; }
        public virtual DbSet<UserContact> UserContacts { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
    }
}
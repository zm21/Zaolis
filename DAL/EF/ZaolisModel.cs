namespace DAL
{
    using DAL.EF;
    using DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ZaolisModel : DbContext
    {
        public ZaolisModel()
            : base("name=Model1")
        {
            Database.SetInitializer(new Initializer());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Avatar> Avatars { get; set; }
        public virtual DbSet<UserContact> UserContacts { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }


    }
}
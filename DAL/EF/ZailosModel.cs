namespace DAL
{
    using DAL.EF;
    using DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ZailosModel : DbContext
    {
        public ZailosModel()
            : base("name=Model1")
        {
            Database.SetInitializer(new Initializer());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Avatar> Avatars { get; set; }
    }
}
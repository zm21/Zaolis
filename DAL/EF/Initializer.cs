using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    class Initializer : DropCreateDatabaseIfModelChanges<ZailosModel>
    {
        protected override void Seed(ZailosModel context)
        {
            base.Seed(context);

            context.Users.AddRange(new List<User>()
            {
                new User() {Login = "admin",Password="admin",Bio="Just an admin"},
            });
            context.SaveChanges();
        }
    }
}

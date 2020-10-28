using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    class Initializer : DropCreateDatabaseIfModelChanges<ZaolisModel>
    {
        protected override void Seed(ZaolisModel context)
        {
            base.Seed(context);

            context.Users.AddRange(new List<User>()
            {
                new User() {Login = "admin",PasswordHash="admin",Bio="Just an admin", Name="Admin", Email="admin@email", IsActive=false},
            });
            context.SaveChanges();
        }
    }
}
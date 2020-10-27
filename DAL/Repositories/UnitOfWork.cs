using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class UnitOfWork : IUnitOfWork
    {
        private ZailosModel context;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Avatar> avatarRepository;
        public IGenericRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new GenericRepository<User>(context);
                return userRepository;
            }
        }

        public IGenericRepository<Avatar> AvatarRepository
        {
            get
            {
                if (avatarRepository == null)
                    avatarRepository = new GenericRepository<Avatar>(context);
                return avatarRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

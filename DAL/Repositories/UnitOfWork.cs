using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ZaolisModel context;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Avatar> avatarRepository;
        private IGenericRepository<UserContact> userContactRepository;
        private IGenericRepository<Attachment> attachmentRepository;
        private IGenericRepository<Message> messageRepository;
        private IGenericRepository<Chat> chatRepository;


        public UnitOfWork(ZaolisModel context)
        {
            this.context = context;
        }

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

        public IGenericRepository<UserContact> UserContactRepository
        {
            get
            {
                if (userContactRepository == null)
                    userContactRepository = new GenericRepository<UserContact>(context);
                return userContactRepository;
            }
        }


        public IGenericRepository<Message> MessageRepository
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new GenericRepository<Message>(context);
                return messageRepository;
            }
        }


        public IGenericRepository<Attachment> AttachmentRepository
        {
            get
            {
                if (attachmentRepository == null)
                    attachmentRepository = new GenericRepository<Attachment>(context);
                return attachmentRepository;
            }
        }

        public IGenericRepository<Chat> ChatRepository
        {
            get
            {
                if (chatRepository == null)
                    chatRepository = new GenericRepository<Chat>(context);
                return chatRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }
    }
}

using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Avatar> AvatarRepository { get; }
        IGenericRepository<UserContact> UserContactRepository { get; }
        IGenericRepository<Message> MessageRepository { get; }
        IGenericRepository<Attachment> AttachmentRepository { get; }
        IGenericRepository<Chat> ChatRepository { get; }
        IGenericRepository<VerificationCode> VerificationCodeRepository { get; }


        void Save();
    }
}

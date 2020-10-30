using AutoMapper;
using BLL.Models;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using EASendMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLClass
    {
        void AddAvatar(AvatarDTO newAvatar);
        void AddCode(VerificationCodeDTO newCode);
        void AddRegistrationCode(RegisterVerificationDTO newCode);
        RegisterVerificationDTO GetRegistrationCode(string email);
        void AddUser(UserDTO newUser);
        void ChangeStatus(UserDTO user,bool status);
        bool IsExistsUserByLoginPassword(string login, string password);
        bool IsExistsUserByEmail(string email);
        UserDTO GetUserByLogin(string login);
        UserDTO GetUserByEmail(string email);
        UserDTO GetUserByLoginAndPassword(string login, string password);
        IEnumerable<UserDTO> GetAllUsers();
    }
    public class BLLClass : IBLLClass
    {
        private IUnitOfWork unit = null;
        private IMapper _mapper = null;
        private string email_login = "zaolisproject@gmail.com";
        private string email_pass = "zaolisqwerty";
        public BLLClass()
        {
            unit = new UnitOfWork(new ZaolisModel());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<Avatar, AvatarDTO>();
                cfg.CreateMap<DAL.Entities.Attachment, AttachmentDTO>();
                cfg.CreateMap<Chat, ChatDTO>();
                cfg.CreateMap<Message, MessageDTO>();
                cfg.CreateMap<UserContact, UserContactDTO>();
                cfg.CreateMap<VerificationCode, VerificationCodeDTO>();
                cfg.CreateMap<RegisterVerification, RegisterVerificationDTO>();

                cfg.CreateMap<UserDTO, User>().ForMember(dest => dest.PasswordHash,
                                               opt => opt.MapFrom(src => Utils.ComputeSha256Hash(src.Password)));

                cfg.CreateMap<AvatarDTO, Avatar>();
                cfg.CreateMap<AttachmentDTO, DAL.Entities.Attachment>();
                cfg.CreateMap<ChatDTO, Chat>();
                cfg.CreateMap<MessageDTO, Message>();
                cfg.CreateMap<UserContactDTO, UserContact>();
                cfg.CreateMap<VerificationCodeDTO, VerificationCode>();
                cfg.CreateMap<RegisterVerificationDTO, RegisterVerification>();
            });

            _mapper = new Mapper(config);
        }
        public void ChangeStatus(UserDTO user,bool status)
        {
            var res=unit.UserRepository.GetById(user.Id);
            if (res != null)
            {
                res.IsActive = status;
                unit.UserRepository.Save();
            }
        }
        public void AddAvatar(AvatarDTO newAvatar)
        {
            unit.AvatarRepository.Create(_mapper.Map<Avatar>(newAvatar));
            unit.AvatarRepository.Save();
        }

        public void AddCode(VerificationCodeDTO newCode)
        {
            unit.VerificationCodeRepository.Create(_mapper.Map<VerificationCode>(newCode));
            unit.VerificationCodeRepository.Save();
        }

        public void AddRegistrationCode(RegisterVerificationDTO newCode)
        {
            unit.RegisterVerificationRepository.Create(_mapper.Map<RegisterVerification>(newCode));
            unit.RegisterVerificationRepository.Save();
        }

        public void AddUser(UserDTO newUser)
        {
            unit.UserRepository.Create(_mapper.Map<User>(newUser));
            unit.UserRepository.Save();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(unit.UserRepository.Get());
        }

        public UserDTO GetUserByLogin(string login)
        {
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u=>u.Login==login))?.First());
        }
        public UserDTO GetUserByLoginAndPassword(string login, string password)
        {
            string passwdhash = Utils.ComputeSha256Hash(password);
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u => u.Login == login&&u.PasswordHash== passwdhash))?.First());
        }

        public bool IsExistsUserByEmail(string email)
        {
            return (_mapper.Map<IEnumerable<User>, UserDTO>(unit.UserRepository.Get(u => u.Email == email))!=null);
        }

        public bool IsExistsUserByLoginPassword(string login, string password)
        {
            string passwdhash = Utils.ComputeSha256Hash(password);
            return _mapper.Map<User>(unit.UserRepository.Get(u => u.Login == login && u.PasswordHash == passwdhash).First()) != null;
        }
        public UserDTO GetUserByEmail(string email)
        {
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u => u.Email == email))?.First());
        }
        
        public void SendRegistrationCode(string email)
        {
            const string server = "smtp.gmail.com";
            Random rnd = new Random();

            SmtpServer smtpserver = new SmtpServer("smtp.gmail.com")
            {
                Port = 465,
                ConnectType = SmtpConnectType.ConnectSSLAuto,
                User = email_login,
                Password = email_pass
            };
            int code = rnd.Next(100000, 999999);
            SmtpMail message = new SmtpMail("TryIt")
            {
                From = email_login,
                To = email,
                Subject = "[Verification Code]",
                TextBody = $"Your verification code for registration in Zaolis Messager: {code}",
                Priority = EASendMail.MailPriority.High
            };

            Task.Run(() => 
            {
                SmtpClient client = new SmtpClient();
                client.Connect(server); 
                client.SendMail(message);
            });

            AddRegistrationCode(new RegisterVerificationDTO() { Code = code, Email = email });
        }

        public RegisterVerificationDTO GetRegistrationCode(string email)
        {
            return _mapper.Map<RegisterVerificationDTO>((unit.RegisterVerificationRepository.Get(u => u.Email == email))?.First());
        }
    }
}
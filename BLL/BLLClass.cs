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
using System.Web;

namespace BLL
{
    public interface IBLLClass
    {
        void AddAvatar(AvatarDTO newAvatar);
        void AddCode(VerificationCodeDTO newCode);
        void AddRegistrationCode(RegisterVerificationDTO newCode);
        void AddAttachment(AttachmentDTO newAttachment);
        RegisterVerificationDTO GetRegistrationCode(string email);
        void AddUser(UserDTO newUser);
        void ChangeStatus(UserDTO user, bool status);
        bool IsExistsUserByLoginPassword(string login, string password);
        bool IsExistsUserByEmail(string email);
        UserDTO GetUserByLogin(string login);
        UserDTO GetUserByEmail(string email);
        UserDTO GetUserByLoginAndPassword(string login, string password);
        IEnumerable<UserDTO> GetAllUsers();
        void SendForgetPassCode(UserDTO user);
        void EditUsersPassword(UserDTO user, string pass);
        void AddMessage(MessageDTO newMessage);
        IEnumerable<UserDTO> GetContacts(UserDTO user);
        void AddContact(UserDTO add_to, UserDTO newContact);
        UserDTO GetUserByName(string name);
        void EditUsersName(UserDTO user, string name);
        void EditUsersBio(UserDTO user, string bio);


    }
    public class BLLClass : IBLLClass
    {
        private IUnitOfWork unit = null;
        private IMapper _mapper = null;
        private string email_login = "zaolisproject@gmail.com";
        private string email_pass = "zaolisqwerty";

       // private string htmlbody = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(@"C:\Users\VitalikOlekS\Desktop\mail.html"));
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
        public void ChangeStatus(UserDTO user, bool status)
        {
            var res = unit.UserRepository.GetById(user.Id);
            if (res != null)
            {
                res.IsActive = status;
                if (status == false)
                    res.LastActive = DateTime.Now;
                unit.UserRepository.Save();
            }
        }
        public void AddAvatar(AvatarDTO newAvatar)
        {
            unit.AvatarRepository.Create(_mapper.Map<Avatar>(newAvatar));
            unit.AvatarRepository.Save();
        }
        public void AddContact(UserDTO add_to,UserDTO newContact)
        {
            var user_res = unit.UserRepository.GetById(add_to.Id);
            if(user_res.UserContact==null)
            {
                UserContact userContact = new UserContact() { UserId = user_res.Id };
                unit.UserContactRepository.Create(userContact);
                unit.Save();
                user_res = unit.UserRepository.GetById(add_to.Id);
            }
            user_res.UserContact.Contacts.Add(unit.UserRepository.GetById(newContact.Id));
            unit.Save();
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
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u => u.Login == login))?.FirstOrDefault());
        }
        public UserDTO GetUserByLoginAndPassword(string login, string password)
        {
            string passwdhash = Utils.ComputeSha256Hash(password);
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u => u.Login == login && u.PasswordHash == passwdhash))?.First());
        }

        public void EditUsersPassword(UserDTO user, string pass)
        {
            if (IsExistsUserByEmail(user.Email))
            {
                unit.UserRepository.GetById(user.Id).PasswordHash = Utils.ComputeSha256Hash(pass);
                unit.Save();
            }
        }
        public void EditUsersName(UserDTO user, string name)
        {
            if (IsExistsUserByEmail(user.Email))
            {
                unit.UserRepository.GetById(user.Id).Name = name;
                unit.Save();
            }
        }

        public void EditUsersBio(UserDTO user, string bio)
        {
            if (IsExistsUserByEmail(user.Email))
            {
                unit.UserRepository.GetById(user.Id).Bio = bio;
                unit.Save();
            }
        }


        public bool IsExistsUserByEmail(string email)
        {
            return (_mapper.Map<UserDTO>(unit.UserRepository.Get(u => u.Email == email).FirstOrDefault()) != null);
        }

        public bool IsExistsUserByLoginPassword(string login, string password)
        {
            string passwdhash = Utils.ComputeSha256Hash(password);
            var res = _mapper.Map<User>(unit.UserRepository.Get(u => u.Login == login && u.PasswordHash == passwdhash).FirstOrDefault());
            return res != null ? true : false;
        }
        public UserDTO GetUserByEmail(string email)
        {
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u => u.Email == email))?.FirstOrDefault());
        }

        public UserDTO GetUserByName(string name)
        {
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u => u.Name == name))?.FirstOrDefault());
        }
        public IEnumerable<UserDTO> GetContacts(UserDTO user)
        {
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(unit.UserRepository.GetById(user.Id)?.UserContact?.Contacts);
        }

        public int SendSystem(string email)
        {
            Random rnd = new Random();
            SmtpServer smtpserver = new SmtpServer("smtp.gmail.com")
            {
                Port = 587,
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
                Priority = EASendMail.MailPriority.High
            };
            message.ImportHtmlBody(@"C:\Users\User-PC\Downloads\mail.html", ImportHtmlBodyOptions.NoOptions);
            message.ImportHtmlBody(@"D:\Downloads\mail.html", ImportHtmlBodyOptions.NoOptions);
            message.HtmlBody=message.HtmlBody.Replace("Your verefication code:", $"Your verefication code:{code}");

            Task.Run(() =>
            {
                SmtpClient client = new SmtpClient();
                client.Connect(smtpserver);
                client.SendMail(message);
            });
            return code;
        }

        public void SendRegistrationCode(string email)
        {
            var res = SendSystem(email);
            AddRegistrationCode(new RegisterVerificationDTO() { Code = res, Email = email });
        }

        public void SendForgetPassCode(UserDTO user)
        {
            var res = SendSystem(user.Email);
            AddCode(new VerificationCodeDTO() { Code = res, CreationTime = DateTime.Now, UserId = user.Id });
        }

        public VerificationCodeDTO GetVerificationCode(string email)
        {
            var res = _mapper.Map<VerificationCodeDTO>((unit.VerificationCodeRepository.Get(u => u.User.Email == email)).FirstOrDefault());
            if (res != null)
            {
                unit.VerificationCodeRepository.Delete(unit.VerificationCodeRepository.GetById(res.Id));
                unit.Save();
            }
            return res;
        }
        public RegisterVerificationDTO GetRegistrationCode(string email)
        {
            var res = _mapper.Map<RegisterVerificationDTO>((unit.RegisterVerificationRepository.Get(u => u.Email == email)).FirstOrDefault());
            unit.RegisterVerificationRepository.Delete(unit.RegisterVerificationRepository.GetById(res.Id));
            unit.Save();
            return res;
        }

        public void AddMessage(MessageDTO newMessage)
        {
            unit.MessageRepository.Create(_mapper.Map<Message>(newMessage));
            unit.MessageRepository.Save();
        }

        public void AddAttachment(AttachmentDTO newAttachment)
        {
            unit.AttachmentRepository.Create(_mapper.Map<DAL.Entities.Attachment>(newAttachment));
            unit.AttachmentRepository.Save();
        }
    }
}
//
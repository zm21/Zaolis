using AutoMapper;
using BLL.Models;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public interface IBLLClass
    {
        void AddAvatar(AvatarDTO newAvatar);
        void AddCode(VerificationCodeDTO newCode);
        void AddRegistrationCode(RegisterVerificationDTO newCode);
        void AddUser(UserDTO newUser);
        bool IsExistsUserByLoginPassword(string login, string password);
        bool IsExistsUserByEmail(string email);
        UserDTO GetUserByLogin(string login);
        IEnumerable<UserDTO> GetAllUsers();
    }
    public class BLLClass : IBLLClass
    {
        private IUnitOfWork unit = null;
        private IMapper _mapper = null;
        public BLLClass()
        {
            unit = new UnitOfWork(new ZaolisModel());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<Avatar, AvatarDTO>();
                cfg.CreateMap<Attachment, AttachmentDTO>();
                cfg.CreateMap<Chat, ChatDTO>();
                cfg.CreateMap<Message, MessageDTO>();
                cfg.CreateMap<UserContact, UserContactDTO>();
                cfg.CreateMap<VerificationCode, VerificationCodeDTO>();
                cfg.CreateMap<RegisterVerification, RegisterVerificationDTO>();

                //cfg.CreateMap<UserDTO, User>().ForMember(dest => dest.PasswordHash,
                //                               opt => opt.MapFrom(src => Utils.ComputeSha256Hash(src.Password)));

                cfg.CreateMap<AvatarDTO, Avatar>();
                cfg.CreateMap<AttachmentDTO, Attachment>();
                cfg.CreateMap<ChatDTO, Chat>();
                cfg.CreateMap<MessageDTO, Message>();
                cfg.CreateMap<UserContactDTO, UserContact>();
                cfg.CreateMap<VerificationCodeDTO, VerificationCode>();
                cfg.CreateMap<RegisterVerificationDTO, RegisterVerification>();
            });

            _mapper = new Mapper(config);
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

        public bool IsExistsUserByEmail(string email)
        {
            return (_mapper.Map<IEnumerable<User>, UserDTO>(unit.UserRepository.Get(u => u.Email == email))!=null);
        }

        public bool IsExistsUserByLoginPassword(string login, string passwdhash)
        {
            //string passwdhash = Utils.ComputeSha256Hash(password);
            return _mapper.Map<IEnumerable<User>, UserDTO>(unit.UserRepository.Get(u => u.Login == login && u.PasswordHash == passwdhash)) != null;
        }
    }
}
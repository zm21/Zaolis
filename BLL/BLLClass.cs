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
        void AddUser(UserDTO newUser);
        void AddChat(ChatDTO newChat);
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
                cfg.CreateMap<Chat, ChatDTO>();
                cfg.CreateMap<UserContact, UserContactDTO>();
                cfg.CreateMap<Attachment, AttachmentDTO>();
                cfg.CreateMap<Message, MessageDTO>();
            });

            _mapper = new Mapper(config);
        }
        public void AddAvatar(AvatarDTO newAvatar)
        {
            unit.AvatarRepository.Create(_mapper.Map<Avatar>(newAvatar));
            unit.AvatarRepository.Save();
        }

        public void AddChat(ChatDTO newChat)
        {
            unit.ChatRepository.Create(_mapper.Map<Chat>(newChat));
            unit.ChatRepository.Save();
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
            return _mapper.Map<UserDTO>((unit.UserRepository.Get(u=>u.Login==login)).First());
        }

        public bool IsExistsUserByEmail(string email)
        {
            return (_mapper.Map<IEnumerable<User>, UserDTO>(unit.UserRepository.Get(u => u.Email == email))!=null);
        }

        public bool IsExistsUserByLoginPassword(string login, string password)
        {
            return _mapper.Map<IEnumerable<User>, UserDTO>(unit.UserRepository.Get(u => u.Login == login&&u.Password==password))!=null;
        }
    }
}

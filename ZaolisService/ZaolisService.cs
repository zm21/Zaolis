using BLL;
using BLL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ZaolisService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ZaolisService : IZaolisService
    {
        BLLClass bll = new BLLClass();
        Dictionary<UserDTO, IZaolisCallback> activeUsers = new Dictionary<UserDTO, IZaolisCallback>();

        public void AddAvatar(AvatarDTO newAvatar)
        {
            bll.AddAvatar(newAvatar);
        }

        public void RegisterUser(string email)
        {
            bll.SendRegistrationCode(email);
        }

        public int GetCodeFromEmail(string email)
        {
            return bll.GetRegistrationCode(email).Code;
        }

        public UserDTO Connect(string login, string password)
        {
            var res = bll.GetUserByLoginAndPassword(login, password);
            if (res != null)
            {
                bll.ChangeStatus(res, true);
                var key = activeUsers.Keys.FirstOrDefault(k => k.Id == res.Id);
                if (key != null)
                    activeUsers[key] = OperationContext.Current.GetCallbackChannel<IZaolisCallback>();
                else
                    activeUsers.Add(res, OperationContext.Current.GetCallbackChannel<IZaolisCallback>());
            }
            return res;
        }

        public UserDTO ConnectByUser(UserDTO user)
        {
            if (user != null)
            {
                bll.ChangeStatus(user, true);
                var key = activeUsers.Keys.FirstOrDefault(k => k.Id == user.Id);
                if (key != null)
                    activeUsers[key] = OperationContext.Current.GetCallbackChannel<IZaolisCallback>();
                else
                    activeUsers.Add(user, OperationContext.Current.GetCallbackChannel<IZaolisCallback>());
            }
            return user;
        }

        public void ChangeCurrentAvatar(UserDTO user)
        {
            bll.ChangeCurrentAvatar(user);
        }

        public void Disconnect(UserDTO user)
        {
            if (user != null)
            {
                bll.ChangeStatus(bll.GetUserByLogin(user.Login), false);
                activeUsers.Remove(user);
            }
        }
        public IEnumerable<AttachmentDTO> GetAttachmentsByChat(ChatDTO chat)
        {
            return bll.GetAttachmentsByChat(chat);
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return bll.GetAllUsers();
        }

        public UserDTO GetUserByLogin(string login)
        {
            return bll.GetUserByLogin(login);
        }

        public bool IsExistsUserByEmail(string email)
        {
            return bll.IsExistsUserByEmail(email);
        }

        public bool IsExistsUserByLoginPassword(string login, string password)
        {
            return bll.IsExistsUserByLoginPassword(login, password);
        }
        public UserDTO GetUserByEmail(string email)
        {
            return bll.GetUserByEmail(email);
        }

        public IEnumerable<UserDTO> GetContacts(UserDTO user)
        {
            return bll.GetContacts(user);
        }

        public int GetVerificationCodeFromEmail(string email)
        {
            return bll.GetVerificationCode(email).Code;
        }
        public void EditUsersPassword(UserDTO user, string pass)
        {
            bll.EditUsersPassword(user, pass);
        }

        public void ForgetPassword(UserDTO user)
        {
            bll.SendForgetPassCode(user);
        }
        public void AddUser(UserDTO user)
        {
            bll.AddUser(user);
        }
        public bool Request()
        {
            return bll.GetVerificationCode("") != null;
        }

        public void SendMessage(MessageDTO message, UserDTO whom)
        {
            var user = activeUsers.Where(u => u.Key.Id == whom.Id)?.FirstOrDefault();
            bll.AddMessage(message);
            if (user?.Key != null)
            {
                activeUsers[user?.Key].RecieveMessage(message);
            }
        }

        public void SendMessageWithAttachment(MessageDTO message, UserDTO whom,AttachmentDTO attachment)
        {
            var user = activeUsers.Where(u => u.Key.Id == whom.Id)?.FirstOrDefault();
            bll.AddMessage(message,attachment);
            if (user?.Key != null)
            {
                activeUsers[user?.Key].RecieveMessageWithAttachment(message,attachment);
            }
        }
        public void AddContact(UserDTO add_to, UserDTO newContact)
        {
            bll.AddContact(add_to, newContact);
        }

        
        public UserDTO GetUserByName(string name)
        {
            return bll.GetUserByName(name);
        }

        public void EditUsersName(UserDTO user, string name)
        {
            bll.EditUsersName(user, name);
        }

        public void EditUsersBio(UserDTO user, string bio)
        {
            bll.EditUsersBio(user, bio);
        }

        public ChatDTO GetChat(UserDTO user, UserDTO contact)
        {
            return bll.GetChat(user, contact);
        }

        public IEnumerable<ChatDTO> GetUserChats(UserDTO user)
        {
            return bll.GetUserChats(user);
        }

        public IEnumerable<UserDTO> GetUsersByChat(ChatDTO chat)
        {
            return bll.GetUsersByChat(chat);
        }

        public AvatarDTO GetAvatar(UserDTO user)
        {
            return bll.GetAvatar(user);
        }

        public IEnumerable<MessageDTO> GetMessagesByChat(ChatDTO chat)
        {
            return bll.GetMessagesByChat(chat);
        }

        public ChatDTO GetChatById(int Id)
        {
            return bll.GetChatById(Id);
        }

        public void RemoveFriendAndChat(UserDTO deletefrom, UserDTO deletewhom, ChatDTO chat)
        {
            bll.RemoveFriendAndChat(deletefrom, deletewhom, chat);
        }

        public UserDTO UpdateUserInfo(UserDTO user)
        {
            var res = bll.GetUserByLogin(user.Login);
            return res;
        }
    }
}

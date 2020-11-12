using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ZaolisService
{
    [ServiceContract(CallbackContract = typeof(IZaolisCallback))]
    public interface IZaolisService
    {
        [OperationContract]
        UserDTO Connect(string login, string passwordHash);

        [OperationContract(IsOneWay = true)]
        void Disconnect(UserDTO user);

        [OperationContract(IsOneWay = true)]
        void AddUser(UserDTO user);

        [OperationContract(IsOneWay = true)]
        void AddAvatar(AvatarDTO newAvatar);

        [OperationContract(IsOneWay = true)]
        void RegisterUser(string email);

        [OperationContract]
        bool IsExistsUserByLoginPassword(string login, string password);

        [OperationContract]
        bool IsExistsUserByEmail(string email);

        [OperationContract]
        UserDTO GetUserByLogin(string login);

        [OperationContract]
        IEnumerable<UserDTO> GetAllUsers();

        [OperationContract]
        UserDTO GetUserByEmail(string email);

        [OperationContract]
        int GetCodeFromEmail(string email);

        [OperationContract]
        int GetVerificationCodeFromEmail(string email);

        [OperationContract(IsOneWay = true)]
        void ForgetPassword(UserDTO user);

        [OperationContract(IsOneWay = true)]
        void EditUsersPassword(UserDTO user, string pass);

        [OperationContract]
        bool Request();

        [OperationContract(IsOneWay = true)]
        void SendMessage(MessageDTO message, UserDTO whom);

        [OperationContract]
        IEnumerable<UserDTO> GetContacts(UserDTO user);

        [OperationContract(IsOneWay = true)]
        void AddContact(UserDTO add_to, UserDTO newContact);

        [OperationContract]
        UserDTO GetUserByName(string name);

        [OperationContract(IsOneWay = true)]
        void EditUsersName(UserDTO user, string name);

        [OperationContract(IsOneWay = true)]
        void EditUsersBio(UserDTO user, string bio);

        [OperationContract]
        ChatDTO GetChat(UserDTO user, UserDTO contact);

        [OperationContract]
        IEnumerable<ChatDTO> GetUserChats(UserDTO user);

        [OperationContract]
        IEnumerable<UserDTO> GetUsersByChat(ChatDTO chat);

        [OperationContract]
        UserDTO UpdateUserInfo(UserDTO user);

        [OperationContract]
        AvatarDTO GetAvatar(UserDTO user);

        [OperationContract]
        UserDTO ConnectByUser(UserDTO user);

        [OperationContract]
        IEnumerable<MessageDTO> GetMessagesByChat(ChatDTO chat);

        [OperationContract]
        ChatDTO GetChatById(int Id);

        [OperationContract(IsOneWay = true)]
        void RemoveFriendAndChat(UserDTO deletefrom, UserDTO deletewhom, ChatDTO chat);

        [OperationContract(IsOneWay = true)]
        void ChangeCurrentAvatar(UserDTO user);

        [OperationContract(IsOneWay = true)]
        void SendMessageWithAttachment(MessageDTO message, UserDTO whom, AttachmentDTO attachment);
        [OperationContract]
        IEnumerable<AttachmentDTO> GetAttachmentsByChat(ChatDTO chat);
    }

    public interface IZaolisCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(MessageDTO message);
        void RecieveMessageWithAttachment(MessageDTO message,AttachmentDTO attachment);
    }

}

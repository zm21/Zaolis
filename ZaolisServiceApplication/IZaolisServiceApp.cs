using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ZaolisServiceApplication
{
    [ServiceContract(CallbackContract = typeof(IZaolisCallback))]
    public interface IZaolisService
    {
        [OperationContract]
        UserDTO Connect(string login, string passwordHash);
        [OperationContract]
        void Disconnect(UserDTO user);
        [OperationContract]
        void AddUser(UserDTO user);
        [OperationContract]
        void AddAvatar(AvatarDTO newAvatar);
        [OperationContract]
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
        [OperationContract]
        void ForgetPassword(UserDTO user);
        [OperationContract]
        void EditUsersPassword(UserDTO user, string pass);
        [OperationContract]
        bool Request();
        [OperationContract]
        void SendMessage(MessageDTO message);
        [OperationContract]
        IEnumerable<UserDTO> GetContacts(UserDTO user);
        [OperationContract]
        void AddContact(UserDTO add_to, UserDTO newContact);
        [OperationContract]
        UserDTO GetUserByName(string name);
        [OperationContract]
        void EditUsersName(UserDTO user, string name);
        [OperationContract]
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
        [OperationContract]
        void RemoveFriendAndChat(UserDTO deletefrom, UserDTO deletewhom, ChatDTO chat);
    }

    public interface IZaolisCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(MessageDTO message);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class TransferFileInto
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Extention { get; set; }
        [DataMember]
        public byte[] data;
        //bool boolValue = true;
        //string stringValue = "Hello ";

        //[DataMember]
        //public bool BoolValue
        //{
        //    get { return boolValue; }
        //    set { boolValue = value; }
        //}

        //[DataMember]
        //public string StringValue
        //{
        //    get { return stringValue; }
        //    set { stringValue = value; }
        //}
    }
}

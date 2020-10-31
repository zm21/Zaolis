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
    }

    public interface IZaolisCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(MessageDTO message);
    }

}

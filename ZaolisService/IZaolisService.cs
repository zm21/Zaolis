using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ZaolisService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IZaolisService" in both code and config file together.
    [ServiceContract]
    public interface IZaolisService
    {
        [OperationContract]
        UserDTO Connect(string login, string passwordHash);
        [OperationContract]
        void Disconnect(UserDTO user);
        [OperationContract]
        void AddAvatar(AvatarDTO newAvatar);
        [OperationContract]
        void AddUser(UserDTO newUser);
        [OperationContract]
        bool IsExistsUserByLoginPassword(string login, string password);
        [OperationContract]
        bool IsExistsUserByEmail(string email);
        [OperationContract]
        UserDTO GetUserByLogin(string login);
        [OperationContract]
        IEnumerable<UserDTO> GetAllUsers();
    }
}

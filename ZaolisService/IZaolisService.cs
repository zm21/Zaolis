using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ZaolisService
{
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
        void RegisterUser(UserDTO newUser);
        [OperationContract]
        bool IsExistsUserByLoginPassword(string login, string password);
        [OperationContract]
        bool IsExistsUserByEmail(string email);
        [OperationContract]
        UserDTO GetUserByLogin(string login);
        [OperationContract]
        IEnumerable<UserDTO> GetAllUsers();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "ZaolisService.ContractType".
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";
    //
    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }
    //
    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}

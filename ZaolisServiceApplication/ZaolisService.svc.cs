using BLL;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ZaolisService : IZaolisService
    {
        BLLClass bll = new BLLClass();
        public void AddAvatar(AvatarDTO newAvatar)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(UserDTO newUser)
        {

        }

        public UserDTO Connect(string login, string password)
        {
            return bll.GetUserByLoginAndPassword(login, password);
        }

        public void Disconnect(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUserByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public bool IsExistsUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsExistsUserByLoginPassword(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}

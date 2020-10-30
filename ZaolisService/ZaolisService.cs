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
    public class ZaolisService : IZaolisService
    {
        BLLClass bll = new BLLClass();
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
            var res= bll.GetUserByLoginAndPassword(login, password);
            if (res != null)
            {
                bll.ChangeStatus(res, true);
            }
            return res;
        }

        public void Disconnect(UserDTO user)
        {
            if(user!=null)
                bll.ChangeStatus(user, false);
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

        public int GetVerificationCodeFromEmail(string email)
        {
            return bll.GetVerificationCode(email).Code;
        }
        public void EditUsersPassword(UserDTO user,string pass)
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
            return bll.GetVerificationCode("")!=null;
        }
    }
}

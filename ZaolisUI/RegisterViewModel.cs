using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ZaolisUI.ZaolisServiceReference;

namespace ZaolisUI
{
    public class RegisterViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        public string Login { get; set; }
        private bool login_valide;
        public string Email { get; set; }
        private bool email_valide;
        public string Passwd { get; set; }
        private bool passwd_valide;
        public string ConfirmPasswd { get; set; }
        private bool confirm_pass_valide;
        private bool isagree_lic;
        ZaolisServiceClient client = new ZaolisServiceClient();
        //public RegisterViewModel()
        //{
        //    Login = "";
        //    Email = "";
        //    Passwd = "";
        //    ConfirmPasswd = "";
        //}
        public bool isAgreeLic
        {
            get { return isagree_lic; }
            set
            {
                isagree_lic = value;
                OnPropertyChanged(nameof(isAllValide));
            }
        }

        public bool isAllValide => (login_valide && email_valide && passwd_valide && confirm_pass_valide && confirm_pass_valide && isAgreeLic);
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (Login != null)
                {
                    switch (columnName)
                    {
                        case nameof(Login):
                            var login_reg = new Regex(@"^\w{3,10}$");
                            if (Login != null && !login_reg.IsMatch(Login))
                            {
                                error = "Login length should be between 3 and 10 characters. Without special characters.";
                                login_valide = false;
                            }
                            else
                            {
                                if (client.GetUserByLogin(Login) != null)
                                {
                                    error = "Such a user already exists!";
                                    login_valide = false;
                                }
                                else
                                    login_valide = true;
                            }
                            OnPropertyChanged(nameof(isAllValide));
                            break;
                        case nameof(Email):
                            {
                                var email_reg = new Regex(@"^[a-z,A-Z,0-9](\.?[a-z,A-Z,0-9]){5,}@[a-z]{2,}\.(com|net|ua|ru)$");
                                if (Email != null && !email_reg.IsMatch(Email))
                                {
                                    error = "Invalid email specified.";
                                    email_valide = false;
                                }
                                else
                                {
                                    if (client.IsExistsUserByEmail(Email))
                                    {
                                        error = "A user with this email already exists!";
                                        email_valide = false;
                                    }
                                    else
                                        email_valide = true;
                                }
                                OnPropertyChanged(nameof(isAllValide));
                                break;
                            }
                        case nameof(Passwd):
                            if (Passwd != null && Passwd.Length < 5)
                            {
                                error = "Password must be at least 5 characters long.";
                                passwd_valide = false;
                            }
                            else
                            {
                                passwd_valide = true;
                            }
                            OnPropertyChanged(nameof(isAllValide));
                            break;
                        case nameof(ConfirmPasswd):
                            if (ConfirmPasswd != null && ConfirmPasswd != Passwd)
                            {
                                error = "Passwords do not match";
                                confirm_pass_valide = false;
                            }
                            else
                            {
                                confirm_pass_valide = true;
                            }
                            OnPropertyChanged(nameof(isAllValide));
                            break;
                        default:
                            break;
                    }
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

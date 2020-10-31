using BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaolisUI.ZaolisServiceReference;

namespace ZaolisUI
{
    class MainMenuViewModel : INotifyPropertyChanged
    {
        ZaolisServiceClient client;
        public ICollection<UserDTO> AllUsers { get; set; }
        public ICollection<UserDTO> FriendUsers { get; set; }
        public UserDTO SelectedUser
        {
            get
            {
                return SelectedUser;
            }
            set
            {
                SelectedUser = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedUser)));
            }
        }
        public MainMenuViewModel()
        {
            client = new ZaolisServiceClient();
            AllUsers = client.GetAllUsers();
            FriendUsers = client.GetAllUsers();//edit
        }
        public void SearchUser(string searchBy)
        {
            if (searchBy != "")
            {
                List<UserDTO> userList = new List<UserDTO>();
                foreach (var item in client.GetAllUsers())
                {
                    if (item.Name.Contains(searchBy))
                    {
                        userList.Add(item);
                    }
                }
                AllUsers = userList;
            }
            else
                AllUsers = client.GetAllUsers();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}

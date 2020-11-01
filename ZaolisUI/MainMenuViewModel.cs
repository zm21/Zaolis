using BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    class MainMenuViewModel : INotifyPropertyChanged
    {
        ZaolisServiceClient.ZaolisServiceClient client;
        public ICollection<UserDTO> AllUsers { get; set; }
        public IEnumerable<UserDTO> FriendUsers { get; set; }
        public UserDTO CurrentUser { get; set; }
        CallbackHandler handler = new CallbackHandler();
        public MainMenuViewModel(UserDTO current)
        {
            CurrentUser = current;
            client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(handler));
            AllUsers = client.GetAllUsers();
            FriendUsers = client.GetContacts(CurrentUser);
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

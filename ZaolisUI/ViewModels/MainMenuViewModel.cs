using BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        ZaolisServiceClient.ZaolisServiceClient client;


        public IEnumerable<UserDTO> FriendUsers { get; set; }

        public ObservableCollection<ChatDTO> Chats { get; set; }

        public UserDTO CurrentUser { get; set; }

        public BitmapSource CurrentAvatar => CurrentUser.AvatarImage();

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; }


        public CallbackHandler handler = new CallbackHandler();

        public MainMenuViewModel(UserDTO current)
        {
            CurrentUser = current;
            client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(handler), "NetTcpBinding_IZaolisService");
            FriendUsers = client.GetContacts(CurrentUser);
            ChatInfos = new ObservableCollection<ChatInfoModel>();

            if (client.GetUserChats(CurrentUser) != null)
            {
                Chats = new ObservableCollection<ChatDTO>(client.GetUserChats(CurrentUser));
                Load();
            }
            //CurrentAvatar = client.GetAvatar(CurrentUser);
        }

        public void DeleteModel(ChatInfoModel model)
        {
            var res = ChatInfos.FirstOrDefault(u => u.ContactMsgGetter.Id == (model).ContactMsgGetter.Id);
            if (model != null && res != null)
            {
                ChatInfos.Remove(res);
            }
        }
        public void SearchUser(string searchBy) 
        {
            //Task.Run(() => { });
            if (searchBy != "")
            {
                
            }
            else 
                Load();
        }
        private void Load() //Loads ChatInfos
        {
            ChatInfos = new ObservableCollection<ChatInfoModel>();
            foreach (var chat in Chats)
            {
                ChatInfos.Add(new ChatInfoModel(client, CurrentUser, chat));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}

using BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    public class ChatInfoModel: INotifyPropertyChanged
    {
        private ZaolisServiceClient.ZaolisServiceClient client;

        private ChatDTO chat;
        public ChatDTO Chat
        {
            get { return chat; }
            set { 
                chat = value;
                OnPropertyChanged(nameof(LastMessage));
                OnPropertyChanged(nameof(SendTime));
            }
        }

        private UserDTO contactMsgGetter;
        public ObservableCollection<MessageModel> Messages { get; set; }
        public BitmapSource CurrentMsgGetterAvatar => contactMsgGetter.AvatarImage();

        
        //Chat friend
        public UserDTO ContactMsgGetter 
        {
            get { return contactMsgGetter; }
            set { 
                contactMsgGetter = value;
                OnPropertyChanged(nameof(OnlineStatus));
            }
        }

        public UserDTO CurrentUser { get; set; }
        public BitmapSource CurrentUserAvatar => CurrentUser.AvatarImage();


        public string LastMessage => Chat.LastMessage != null ? Chat.LastMessage.MessageText.Length>10?Chat.LastMessage.MessageText.Substring(0,7)+"...":Chat.LastMessage.MessageText : "No messages here yet.";

        public string SendTime => Chat.LastMessage != null ? Chat.LastMessage.CreationTime.ToShortTimeString() : "";

        public string OnlineStatus => ContactMsgGetter.IsActive ? "Online" : ContactMsgGetter.LastActive.ToShortTimeString();

        public ChatInfoModel(ZaolisServiceClient.ZaolisServiceClient client,UserDTO current,ChatDTO chat)
        {
            this.client = client;
            Chat = chat;
            CurrentUser = current;

            ContactMsgGetter = client.GetUsersByChat(Chat).Where(u=>u.Id!=current.Id).FirstOrDefault();
            Messages = new ObservableCollection<MessageModel>();

            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in client.GetMessagesByChat(chat))
                    {
                        Messages.Add(new MessageModel(item, CurrentUser) { Message = item });
                    }
                    if (Messages.Count() > 1)
                    {
                        var firstMsg = Messages.First();

                        Messages.Remove(firstMsg);
                        Messages.Insert(Messages.IndexOf(Messages.Last()) + 1, firstMsg);
                    }
                });
            });
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void UpdateChat()
        {
            Chat = client.GetChatById(chat.Id);
        }
    }
}

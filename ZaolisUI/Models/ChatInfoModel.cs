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
        //Chat companion
        public UserDTO ContactMsgGetter 
        {
            get { return contactMsgGetter; }
            set { 
                contactMsgGetter = value;
                OnPropertyChanged(nameof(OnlineStatus));
            }
        }
        public UserDTO CurrentUser { get; set; }

        public string LastMessage => Chat.LastMessage != null ? Chat.LastMessage.MessageText.Length>17?Chat.LastMessage.MessageText.Substring(0,17)+"...":Chat.LastMessage.MessageText : "No messages here yet.";

        public string SendTime => Chat.LastMessage != null ? Chat.LastMessage.CreationTime.ToShortTimeString() : "";

        public string OnlineStatus => contactMsgGetter.IsActive ? "Online" : contactMsgGetter.LastActive.ToShortTimeString();

        public ChatInfoModel(ZaolisServiceClient.ZaolisServiceClient client,UserDTO current,ChatDTO chat)
        {
            this.client = client;
            Chat = chat;
            CurrentUser = current;
            ContactMsgGetter = client.GetUsersByChat(Chat).Where(u=>u.Id!=current.Id).FirstOrDefault();
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in client.GetMessagesByChat(chat))
                    {
                        Messages.Add(new MessageModel(item, CurrentUser) { Message = item });
                    }
                });
            });
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ChatMessageListItemViewModel 
    {
        public string SenderName { get; set; }

        public string Message { get; set; }

        public string Initials { get; set; }

        public string ProfilePictureRGB { get; set; }

        public bool IsSelected { get; set; }

        public bool SentByMe { get; set; }

        public DateTimeOffset MessageReadTime { get; set; }

        public bool MessageRead => MessageReadTime > DateTimeOffset.MinValue;

        public DateTimeOffset MessageSentTime { get; set; }

        public bool NewItem { get; set; }

        public bool HasMessage => Message != null;
    }
}

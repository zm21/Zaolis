using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    public class ChatInfoModel
    {
        private ZaolisServiceClient.ZaolisServiceClient client;
        public ChatDTO Chat { get; set; }
        public UserDTO ContactMsgGetter { get; set; }  //Chat companion
        public UserDTO Current { get; set; }
        public string LastMessage => Chat.LastMessage!=null? Chat?.LastMessage?.MessageText?.Substring(0, 17)+"...":"No messages here yet.";
        public string SendTime => Chat.LastMessage != null ? Chat.LastMessage.CreationTime.ToShortTimeString() : "";
        public ChatInfoModel(ZaolisServiceClient.ZaolisServiceClient client,UserDTO current,ChatDTO chat)
        {
            this.client = client;
            Chat = chat;
            Current = current;
            ContactMsgGetter = client.GetUsersByChat(Chat).Where(u=>u.Id!=current.Id).FirstOrDefault();
        }
    }
}

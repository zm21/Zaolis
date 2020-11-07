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
        private ChatDTO Chat { get; set; }
        private UserDTO MessageGetter { get; set; }
        private UserDTO Current { get; set; }
        public ChatInfoModel(ZaolisServiceClient.ZaolisServiceClient client,UserDTO current)
        {
            this.client = client;
            Current = current;
            MessageGetter=client;
        }
    }
}

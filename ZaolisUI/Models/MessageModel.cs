using BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaolisUI
{
    public class MessageModel
    {
        public MessageDTO Message { get; set; }
        public bool SentByMe { get; }
        private UserDTO CurrentUser { get; }
        public MessageModel(MessageDTO message,UserDTO userDTO)
        {
            this.CurrentUser = userDTO;
            this.Message = message;
            if (message.UserId == CurrentUser.Id)
                SentByMe = true;
        }
    }
}

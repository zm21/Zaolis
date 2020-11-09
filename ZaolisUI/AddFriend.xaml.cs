using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for AddFriend.xaml
    /// </summary>
    public partial class AddFriend : Window
    {
        UserDTO user;
        CallbackHandler handler=new CallbackHandler();
        ZaolisServiceClient.ZaolisServiceClient client;
        public AddFriend(UserDTO user, ZaolisServiceClient.ZaolisServiceClient client)
        {
            InitializeComponent();
            this.user = user;
            this.client = client;
            this.client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(handler));
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtBox_username.Text!=null)
            {
                var res = client.GetUserByLogin(txtBox_username.Text);
                if(res!=null)
                {
                    client.AddContact(user, res);
                    client.GetChat(user, res);
                }
                else
                {
                    MsgBox msg = new MsgBox("Error", "There is no such user in our system!");
                    msg.Show();
                }
            }
        }
    }
}

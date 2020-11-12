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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl, IChildWindow
    {
        private DockPanel parent;

        private ChatInfoModel model;

        private UserDTO CurrentUser;

        private MainMenuViewModel viewModel;

        private ZaolisServiceClient.ZaolisServiceClient client;
        public UserInfo(DockPanel parent,ChatInfoModel model,ZaolisServiceClient.ZaolisServiceClient client,ref MainMenuViewModel viewModel,bool nightMode)
        {
            InitializeComponent();

            CurrentUser = model.ContactMsgGetter;

            this.client = client;
            this.model = model;
            this.viewModel = viewModel;
            this.parent = parent;
            this.DataContext = model;
            

            if(nightMode)
            {
                var converter = new BrushConverter();
                labelUserInfo.Foreground = Brushes.White;
                textBlockBio.Foreground = Brushes.White;
                textBlockEmail.Foreground = Brushes.White;
                textBlockUsername.Foreground = Brushes.White;
                textBlockMainUserName.Foreground = Brushes.White;
                textBlockNotification.Foreground = Brushes.White;
                iconBio.Foreground = Brushes.LightGray;
                iconEmail.Foreground = Brushes.LightGray;
                iconDelete.Foreground = Brushes.LightGray;
                iconNotification.Foreground = Brushes.LightGray;
                iconUsername.Foreground = Brushes.LightGray;
                mainDockPanel.Background= (Brush)converter.ConvertFromString("#17212B");
                border1.Background = (Brush)converter.ConvertFromString("#232E3C");
                border2.Background = (Brush)converter.ConvertFromString("#232E3C");
            }
        }

        public event ClosingDelegate Closing;
        public event MessageDelegate OpenMsg;

        public void Close()
        {
            parent.Children.Remove(this);
        }

        public void ShowMsg(string title, string msg)
        {
            throw new NotImplementedException();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void buttonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteModel(model);
            client.RemoveFriendAndChat(model.CurrentUser, model.ContactMsgGetter, model.Chat);
        }
    }
}

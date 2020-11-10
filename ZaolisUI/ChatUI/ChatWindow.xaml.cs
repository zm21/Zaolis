using BLL.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Shell;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : UserControl
    {
        public ChatInfoModel ChatInfo { get; private set; }

        public ChatDTO Chat => ChatInfo.Chat;

        private ZaolisServiceClient.ZaolisServiceClient client;

        private DockPanel OverlayDockPanel;

        public MessageModel MessageModel { get; private set; }

        private MainMenuViewModel viewModel;

        public ChatWindow(ChatInfoModel chatInfoModel, ZaolisServiceClient.ZaolisServiceClient client,DockPanel dockPanel, MainMenuViewModel viewModel)
        {
            InitializeComponent();

            ScrollViewer.ScrollToEnd();

            this.ChatInfo = chatInfoModel;
            this.DataContext = ChatInfo;
            this.client = client;
            this.viewModel = viewModel;

            OverlayDockPanel = dockPanel;

            Task.Run(() => 
            {
                while(true)
                {
                    ChatInfo.ContactMsgGetter = client.UpdateUserInfo(ChatInfo.ContactMsgGetter);
                    Thread.Sleep(10000);
                }
            });
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtbox_message.Text))
            {
                MessageDTO messageDTO = new MessageDTO();

                messageDTO.ChatId = ChatInfo.Chat.Id;
                messageDTO.MessageText = txtbox_message.Text;
                messageDTO.CreationTime = DateTime.Now;
                messageDTO.UserId = ChatInfo.CurrentUser.Id;

                client.SendMessageAsync(messageDTO);

                txtbox_message.Text = "";
            }
        }

        private void DockPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UserInfo userInfo = new UserInfo(OverlayDockPanel,ChatInfo,client, viewModel);
            OverlayDockPanel.Children.Add(userInfo);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png";
            openFileDialog.ShowDialog();
        }
    }
}

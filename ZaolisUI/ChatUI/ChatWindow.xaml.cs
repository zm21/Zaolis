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

        private bool nightMode;

        public ChatWindow(ChatInfoModel chatInfoModel, ZaolisServiceClient.ZaolisServiceClient client, DockPanel dockPanel, ref MainMenuViewModel viewModel)
        {
            InitializeComponent();

            ScrollViewer.ScrollToEnd();

            this.ChatInfo = chatInfoModel;
            this.DataContext = ChatInfo;
            this.client = client;
            this.viewModel = viewModel;
            this.nightMode = viewModel.NightMode;

            OverlayDockPanel = dockPanel;



            Task.Run(() =>
            {
                while (true)
                {
                    ChatInfo.ContactMsgGetter = client.UpdateUserInfo(ChatInfo.ContactMsgGetter);
                    Thread.Sleep(10000);
                }
            });
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbox_message.Text))
            {
                MessageDTO messageDTO = new MessageDTO();

                messageDTO.ChatId = ChatInfo.Chat.Id;
                messageDTO.MessageText = txtbox_message.Text;
                messageDTO.CreationTime = DateTime.Now;
                messageDTO.UserId = ChatInfo.CurrentUser.Id;

                ChatInfo.Messages.Add(new MessageModel(messageDTO, ChatInfo.CurrentUser));

                client.SendMessageAsync(messageDTO);

                txtbox_message.Text = "";
            }
        }

        private void DockPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool nm = false;
            foreach (var item in (OverlayDockPanel.Parent as Grid).Children)
            {
                if (item is DockPanel)
                {
                    if ((item as DockPanel).Name == "TopGrid")
                    {
                        if ((item as DockPanel).Background.ToString() == "#FF03A9F4")
                        {
                            nm = true;
                            break;
                        }
                    }
                }
            }
            if (nm)
            {
                UserInfo userInfo = new UserInfo(OverlayDockPanel, ChatInfo, client, ref viewModel, false);
                OverlayDockPanel.Children.Add(userInfo);
            }
            else
            {
                UserInfo userInfo = new UserInfo(OverlayDockPanel, ChatInfo, client, ref viewModel, true);
                OverlayDockPanel.Children.Add(userInfo);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png";
            openFileDialog.ShowDialog();
        }


        private void txtbox_message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonSend_Click(sender, new RoutedEventArgs());
        }
        
        public void UpdateUI()
        {
            bool nm = false;
            var converter = new BrushConverter();
            foreach (var item in (OverlayDockPanel.Parent as Grid).Children)
            {
                if (item is DockPanel)
                {
                    if ((item as DockPanel).Name == "TopGrid")
                    {
                        if ((item as DockPanel).Background.ToString() == "#FF03A9F4")
                        {
                            nm = true;
                            break;
                        }
                    }
                }
            }
            if (nm)
            {
                userInfo.Background = (Brush)converter.ConvertFromString("#17212B");
                dockPanelMessage.Background = (Brush)converter.ConvertFromString("#17212B");
                userTextName.Foreground = Brushes.White;
                userTextStatus.Foreground = Brushes.LightGray;
                txtbox_message.Foreground = Brushes.White;
                buttonAttachments.Foreground = Brushes.LightGray;
                buttonMic.Foreground = Brushes.LightGray;
                buttonSend.Foreground = Brushes.LightGray;
                buttonSticker.Foreground = Brushes.LightGray;
            }
            else
            {
                userInfo.Background = (Brush)converter.ConvertFromString("#C8E4F8");
                dockPanelMessage.Background = (Brush)converter.ConvertFromString("#C8E4F8");
                userTextName.Foreground = Brushes.Black;
                userTextStatus.Foreground = Brushes.Gray;
                txtbox_message.Foreground = Brushes.Black;
                buttonAttachments.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonMic.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonSend.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonSticker.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
            }
        }
    }

}

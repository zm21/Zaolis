using BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for MainMenuZaolis.xaml
    /// </summary>
    public partial class MainMenuZaolis : Window
    {
        private ZaolisServiceClient.ZaolisServiceClient client;

        private UserDTO loginnedUser;

        private MainMenuViewModel mainMenuViewModel;

        private ObservableCollection<UserDTO> users;

        private CallbackHandler callbackHandler;

        private IChatManager chatManager;


        #region Tray&Notifications
        //Tray&Notifications
        private System.Windows.Forms.NotifyIcon m_notifyIcon;

        private System.Windows.Forms.MenuItem menuItem;

        private System.Windows.Forms.MenuItem menuItem1;

        private System.Windows.Forms.ContextMenu contextMenu;
        #endregion
        public MainMenuZaolis(ObservableCollection<UserDTO> users, ZaolisServiceClient.ZaolisServiceClient client)
        {
            InitializeComponent();


            this.users = users;

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            this.client = client;

            loginnedUser = this.users.First();
            mainMenuViewModel = new MainMenuViewModel(loginnedUser);

            chatManager = new ChatManager(10, ref mainMenuViewModel);

            this.DataContext = mainMenuViewModel;

            foreach (var item in users)
            {
                logginedUsers.Items.Add(item);
            }

            logginedUsers.SelectedItem = loginnedUser;

            callbackHandler = new CallbackHandler();
            callbackHandler.RecieveEvent += CallbackHandler_RecieveEvent;

            client.Disconnect(loginnedUser);
            client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(callbackHandler), "NetTcpBinding_IZaolisService");
            client.ConnectByUser(loginnedUser);
            Tray();
            Notification("Zaolis", "Zaolis is working on the tray");
        }

        private void Tray()
        {
            this.contextMenu = new System.Windows.Forms.ContextMenu();

            this.menuItem = new System.Windows.Forms.MenuItem();
            this.menuItem.Index = 0;
            this.menuItem.Text = "Quit Zaolis";
            this.menuItem.Click += new EventHandler(menuItem1_Click);

            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "Open Zaolis";
            this.menuItem1.Click += new EventHandler(menuItem_Click);

            contextMenu.MenuItems.Add(menuItem1);
            contextMenu.MenuItems.Add(menuItem);
        }
        private void Notification(string title, string msg)
        {
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();

            m_notifyIcon.Icon = new System.Drawing.Icon(@"..\..\Resources\z.ico");
            m_notifyIcon.BalloonTipText = msg;
            m_notifyIcon.BalloonTipClicked += new EventHandler(m_notifyIcon_Click);
            m_notifyIcon.BalloonTipTitle = title;
            m_notifyIcon.Text = "Zaolis";
            m_notifyIcon.ContextMenu = contextMenu;

            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);
        }
        private void CallbackHandler_RecieveEvent(MessageDTO obj)
        {
            var chat = mainMenuViewModel.Chats.FirstOrDefault(c => c.Id == obj.ChatId);
            var chatInfo = mainMenuViewModel.ChatInfos.Where(u => u.ContactMsgGetter.Id == obj.UserId).FirstOrDefault();
            chatInfo?.UpdateChat();
            Notification(chatInfo?.ContactMsgGetter?.Name, chatInfo.LastMessage);
            m_notifyIcon.Visible = true;
            m_notifyIcon.ShowBalloonTip(10000);
            chatInfo.Messages.Add(new MessageModel(obj, loginnedUser));
            //mainMenuViewModel.Chats.Insert(0, client.GetChatById(chat.Id));
            //mainMenuViewModel.Chats.Remove(chat);
        }

        private void TopGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (logginedUsers != null)
                client.Disconnect(loginnedUser);

            m_notifyIcon.Dispose();
            m_notifyIcon = null;
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainMenuViewModel.SearchUser(textBoxSearch.Text);
        }

        private void buttonNightMode_Click(object sender, RoutedEventArgs e)
        {
            chatManager.UpdateUI();
            toggleButtonNightMode.IsChecked = !toggleButtonNightMode.IsChecked;

            var converter = new BrushConverter();


            if (mainMenuViewModel.NightMode)
            {
                TopGrid.Background = (Brush)converter.ConvertFromString("#1F2935");
                foreach (var item in this.TopGrid.Children)
                {
                    if (item is Button)
                    {
                        (item as Button).Foreground = Brushes.LightGray;
                    }
                }

                //Menu
                topDockPanelInMenu.Background = (Brush)converter.ConvertFromString("#276899");
                GridMenu.Background = (Brush)converter.ConvertFromString("#17212B");
                expanderAccounts.Background = (Brush)converter.ConvertFromString("#276899");
                expanderAccounts.Foreground = Brushes.White;
                logginedUsers.Background = (Brush)converter.ConvertFromString("#17212B");
                logginedUsers.ItemTemplate = (DataTemplate)Resources["DarkAccountTemplate"];
                //Buttons
                labelAddAccount.Foreground = Brushes.White;
                labelFindFriend.Foreground = Brushes.White;
                labelLogout.Foreground = Brushes.White;
                labelNewGroup.Foreground = Brushes.White;
                labelSettings.Foreground = Brushes.White;
                labelNightMode.Foreground = Brushes.White;
                buttonSettings.Foreground = Brushes.LightGray;
                buttonAddAccount.Foreground = Brushes.LightGray;
                buttonLogout.Foreground = Brushes.LightGray;
                buttonCreateNewChat.Foreground = Brushes.LightGray;
                buttonFindFriend.Foreground = Brushes.LightGray;
                buttonNightMode.Foreground = Brushes.LightGray;

                //ChatList
                chatListDockPanel.Background = (Brush)converter.ConvertFromString("#17212B");
                lbox_chats.Background = (Brush)converter.ConvertFromString("#17212B");
                textBoxSearch.Background = (Brush)converter.ConvertFromString("#232F3D");
                textBoxSearch.Foreground = Brushes.White;
                textBoxSearch.BorderBrush = Brushes.LightGray;
                lbox_chats.ItemTemplate = (DataTemplate)Resources["DarkChatTemplate"];
                //Chat
                ChatPanel.Background = (Brush)converter.ConvertFromString("#0E1621");
                gridSplitter.Background = Brushes.Black;
            }
            else
            {
                TopGrid.Background = (Brush)converter.ConvertFromString("#03A9F4");
                foreach (var item in this.TopGrid.Children)
                {
                    if (item is Button)
                    {
                        (item as Button).Foreground = Brushes.LightGray;
                    }
                }

                //Menu
                topDockPanelInMenu.Background = Brushes.White;
                GridMenu.Background = Brushes.White;
                expanderAccounts.Background = null;
                expanderAccounts.Foreground = Brushes.Black;
                logginedUsers.Background = null;
                logginedUsers.ItemTemplate = (DataTemplate)Resources["LightAccountTemplate"];
                //Buttons
                labelAddAccount.Foreground = Brushes.Black;
                labelFindFriend.Foreground = Brushes.Black;
                labelLogout.Foreground = Brushes.Black;
                labelNewGroup.Foreground = Brushes.Black;
                labelSettings.Foreground = Brushes.Black;
                labelNightMode.Foreground = Brushes.Black;
                buttonSettings.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonAddAccount.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonLogout.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonCreateNewChat.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonFindFriend.Foreground = (Brush)converter.ConvertFromString("#03A9F4");
                buttonNightMode.Foreground = (Brush)converter.ConvertFromString("#03A9F4");

                //ChatList
                chatListDockPanel.Background = Brushes.White;
                lbox_chats.Background = null;
                textBoxSearch.Foreground = Brushes.Black;
                textBoxSearch.Background = (Brush)converter.ConvertFromString("#C8E4F8");
                lbox_chats.ItemTemplate = (DataTemplate)Resources["LightChatTemplate"];
                //Chat
                ChatPanel.Background = Brushes.White;
                gridSplitter.Background = Brushes.LightGray;
            }
            
        }

        private void buttonFindFriend_Click(object sender, RoutedEventArgs e)
        {
            AddFriend add = new AddFriend(loginnedUser, client, ref mainMenuViewModel);

            add.Owner = this;
            add.ShowDialog();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Settings settings = new Settings(OverlayDockPanel, client, loginnedUser, users,mainMenuViewModel.NightMode);

                    OverlayDockPanel.Children.Add(settings);
                });
            });

        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            if (logginedUsers.SelectedItem != null)
            {
                var user = (UserDTO)logginedUsers.SelectedItem;

                client.Disconnect(user);
                users.Remove(user);

                logginedUsers.Items.Remove(user);
                SignInUpWindow.Save(users);
            }
            if (logginedUsers.Items.Count <= 0)
            {
                SignInUpWindow signInUpWindow = new SignInUpWindow();
                signInUpWindow.Show();

                this.Close();
            }
        }

        private void buttonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            SignInUpWindow signInUpWindow = new SignInUpWindow(users, client);
            signInUpWindow.Show();

            this.Close();
        }

        private void logginedUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (logginedUsers.SelectedItem != null)
                    {
                        loginnedUser = (UserDTO)logginedUsers.SelectedItem;

                        mainMenuViewModel = new MainMenuViewModel(loginnedUser);
                        this.DataContext = mainMenuViewModel;
                    }
                });
            });

        }

        private void lbox_chats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbox_chats.SelectedItem != null)
            {
                ChatPanel.Children.Clear();

                var chatInfo = (lbox_chats.SelectedItem as ChatInfoModel);

                chatManager.LoadChat(chatInfo, client, OverlayDockPanel);
                ChatPanel.Children.Add(chatManager.GetChatWindow(chatInfo.Chat));
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            textBoxSearch.IsEnabled = false;
            ButtonOpenMenu.IsEnabled = false;

            GridBackground.IsEnabled = true;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            GridBackground.IsEnabled = false;
            textBoxSearch.IsEnabled = true;
            ButtonOpenMenu.IsEnabled = true;
        }

        private void ButtonCloseMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            ButtonCloseMenu.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void ButtonCloseMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonCloseMenu.Foreground = new SolidColorBrush(Color.FromArgb(255, 33, 150, 243));
        }

        private void GridBackground_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GridBackground.IsEnabled = false;
            textBoxSearch.IsEnabled = true;
            ButtonOpenMenu.IsEnabled = true;
        }


        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            m_notifyIcon.Visible = false;
            WindowState = WindowState.Normal;
        }


        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
        private void menuItem_Click(object Sender, EventArgs e)
        {
            this.Show();
            m_notifyIcon.Visible = false;
            WindowState = WindowState.Normal;
        }
    }
    public interface IChatManager
    {
        void LoadChat(ChatInfoModel chatInfoModel, ZaolisServiceClient.ZaolisServiceClient client, DockPanel OverlayDockPanel);

        //remove all loaded chats
        void Free();
        bool IsLoadedChat(ChatDTO chatDTO);
        ChatWindow GetChatWindow(ChatDTO chatDTO);
        void UpdateUI();
    }
    public class ChatManager : IChatManager
    {
        private List<ChatWindow> chatWindows;
        public int MaxCount { get; private set; }

        private MainMenuViewModel viewModel;
        public ChatManager(int MaxCount, ref MainMenuViewModel viewModel)
        {
            chatWindows = new List<ChatWindow>();

            this.viewModel = viewModel;
            this.MaxCount = MaxCount;
        }
        public void Free()
        {
            chatWindows.Clear();
        }
        public bool IsLoadedChat(ChatDTO chatDTO)
        {
            return chatWindows.FirstOrDefault(c => c.Chat.Id == chatDTO.Id) != null;
        }
        public void LoadChat(ChatInfoModel chatInfoModel, ZaolisServiceClient.ZaolisServiceClient client, DockPanel OverlayDockPanel)
        {
            if (IsLoadedChat(chatInfoModel.Chat))
            {
                var chatwindow = chatWindows.FirstOrDefault(c => c.Chat.Id == chatInfoModel.Chat.Id);

                chatWindows.Remove(chatwindow);
                chatWindows.Insert(0, chatwindow);
            }
            else
            {
                //Messages sorting
                if (chatWindows.Count == MaxCount)
                    chatWindows.RemoveAt(MaxCount - 1);
                
                chatWindows.Insert(0, new ChatWindow(chatInfoModel, client, OverlayDockPanel, ref viewModel));
            }
        }

        public ChatWindow GetChatWindow(ChatDTO chatDTO)
        {
            var chatwindow = chatWindows.FirstOrDefault(c => c.Chat.Id == chatDTO.Id);
            return chatwindow;
        }

        public void UpdateUI()
        {
            foreach (var item in chatWindows)
            {
                item.UpdateUI();
            }
        }
    }
    public class CallbackHandler : IZaolisServiceCallback
    {
        public event Action<MessageDTO> RecieveEvent;
        public void RecieveMessage(MessageDTO message)
        {
            RecieveEvent?.Invoke(message);
        }
    }




}

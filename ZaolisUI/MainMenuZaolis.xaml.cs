using BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private IChatManager chatManager;
        public MainMenuZaolis(ObservableCollection<UserDTO> users, ZaolisServiceClient.ZaolisServiceClient client)
        {
            InitializeComponent();
            this.users = users;

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            this.client = client;

            loginnedUser = this.users.First();
            client.ConnectByUser(loginnedUser);

            mainMenuViewModel = new MainMenuViewModel(loginnedUser);

            chatManager = new ChatManager(10);

            this.DataContext = mainMenuViewModel;

            foreach (var item in users)
            {
                logginedUsers.Items.Add(item);
                //client.AddAvatar(new AvatarDTO() { Path = "default.png", UserId = item.Id });
            }

            logginedUsers.SelectedItem = loginnedUser;
        }

        private void TopGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainMenuViewModel.SearchUser(textBoxSearch.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserInfo usInfo = new UserInfo(OverlayDockPanel); 
            OverlayDockPanel.Children.Add(usInfo);
        }

        private void buttonNightMode_Click(object sender, RoutedEventArgs e)
        {
            toggleButtonNightMode.IsChecked = !toggleButtonNightMode.IsChecked;
        }

        private void buttonFindFriend_Click(object sender, RoutedEventArgs e)
        {
            AddFriend add = new AddFriend(loginnedUser, client);
            add.Owner = this;
            add.ShowDialog();
        }
        
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Settings settings = new Settings(OverlayDockPanel, client, loginnedUser,users);
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
            if(lbox_chats.SelectedItem!=null)
            {
                ChatPanel.Children.Clear();
                var chatInfo = (lbox_chats.SelectedItem as ChatInfoModel);
                chatManager.LoadChat(chatInfo,client);
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
    }
    public interface IChatManager
    {
        void LoadChat(ChatInfoModel chatInfoModel, ZaolisServiceClient.ZaolisServiceClient client);
        //remove all loaded chats
        void Free();
        bool IsLoadedChat(ChatDTO chatDTO);
        ChatWindow GetChatWindow(ChatDTO chatDTO);
    }
    public class ChatManager : IChatManager
    {
        private List<ChatWindow> chatWindows;
        public int MaxCount { get; private set; }
        public ChatManager(int MaxCount)
        {
            chatWindows = new List<ChatWindow>();
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
        public void LoadChat(ChatInfoModel chatInfoModel, ZaolisServiceClient.ZaolisServiceClient client)
        {
            if(IsLoadedChat(chatInfoModel.Chat))
            {
                var chatwindow = chatWindows.FirstOrDefault(c => c.Chat.Id == chatInfoModel.Chat.Id);
                chatWindows.Remove(chatwindow);
                chatWindows.Insert(0, chatwindow);
            }
            else
            {
                if (chatWindows.Count == MaxCount)
                    chatWindows.RemoveAt(MaxCount - 1);
                chatWindows.Insert(0,new ChatWindow(chatInfoModel, client));
            }
        }

        public ChatWindow GetChatWindow(ChatDTO chatDTO)
        {
            var chatwindow = chatWindows.FirstOrDefault(c => c.Chat.Id == chatDTO.Id);
            return chatwindow;
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

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
        ZaolisServiceClient.ZaolisServiceClient client;
        UserDTO loginnedUser;
        MainMenuViewModel mainMenuViewModel;
        ObservableCollection<UserDTO> users;
        public MainMenuZaolis(ObservableCollection<UserDTO> users, ZaolisServiceClient.ZaolisServiceClient client)
        {
            InitializeComponent();
            this.users = users;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.client = client;
            loginnedUser = this.users.First();
            mainMenuViewModel = new MainMenuViewModel(loginnedUser);
            this.DataContext = mainMenuViewModel;
            foreach (var item in users)
            {
                logginedUsers.Items.Add(item);
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
        //
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Settings settings = new Settings(OverlayDockPanel, client, loginnedUser);
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
            ButtonCloseMenu.Foreground = new SolidColorBrush(Color.FromArgb(255,33,150,243));
        }

        private void GridBackground_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GridBackground.IsEnabled = false;
            textBoxSearch.IsEnabled = true;
            ButtonOpenMenu.IsEnabled = true;
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

using BLL.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml;
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl, IChildWindow, IOverlayWindow
    {
        private static string remembered_path = "rememberme.dat";

        private DockPanel parent;

        private ZaolisServiceClient.ZaolisServiceClient client;

        private UserDTO user;

        private MainMenuViewModel mainMenuViewModel;

        private ObservableCollection<UserDTO> users;

        public Settings(DockPanel mainGrid, ZaolisServiceClient.ZaolisServiceClient client, UserDTO user, ObservableCollection<UserDTO> users, bool nightMode)
        {
            InitializeComponent();
            this.client = client;
            this.user = user;

            parent = mainGrid;
            mainMenuViewModel = new MainMenuViewModel(user);

            this.users = users;
            this.DataContext = mainMenuViewModel;
            if(nightMode)
            {
                var converter = new BrushConverter();
                mainDockPanel.Background = (Brush)converter.ConvertFromString("#17212B");
                avatarDockPanel.Background = (Brush)converter.ConvertFromString("#232E3C");
                borderSplitter1.Background= (Brush)converter.ConvertFromString("#232E3C");
                nameTextBox.Foreground = Brushes.White;
                bioTextBox.Foreground = Brushes.White;
                labelSettings.Foreground = Brushes.White;
                iconName.Foreground = Brushes.LightGray;
                labelExample1.Foreground = Brushes.LightGray;
                labelExample2.Foreground = Brushes.LightGray;
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
            MsgBox msgBox = new MsgBox(title, msg);
            msgBox.Show();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (nameTextBox.Text != null)
                    {
                        client.EditUsersName(users.Where(u => u.Login == user.Login).FirstOrDefault(), nameTextBox.Text);

                        if (File.Exists(remembered_path))
                        {
                            SignInUpWindow.Save(users);
                        }
                    }
                });
            });
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (bioTextBox.Text != null)
                    {
                        client.EditUsersBio(users.Where(u => u.Login == user.Login).FirstOrDefault(), bioTextBox.Text);

                        if (File.Exists(remembered_path))
                        {
                            SignInUpWindow.Save(users);
                        }
                    }
                });
            });
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void buttonClose_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonClose.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void buttonClose_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonClose.Foreground = new SolidColorBrush(Color.FromArgb(255, 33, 150, 243));
        }

        private void buttonSetProfilePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "D:\\avatars";

            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if(openFileDialog.ShowDialog()==true)
            {
                int index = users.IndexOf(users.Where(u=>u.Login==user.Login).FirstOrDefault());
                users.RemoveAt(index);


                client.ChangeCurrentAvatar(user);
                client.AddAvatar(new AvatarDTO() { IsActive = true, Path = openFileDialog.FileName, UserId = user.Id });
                users.Insert(index, client.GetUserByLogin(user.Login));
                mainMenuViewModel.CurrentUser = client.GetUserByLogin(user.Login);
                ShowMsg("Avatar", "Your avatar was updated!");

                SignInUpWindow.Save(users);
            }
        }
    }
}

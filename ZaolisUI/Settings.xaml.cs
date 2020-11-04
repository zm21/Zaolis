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
using ZaolisUI.ZaolisServiceClient;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl, IChildWindow
    {
        Grid grid;
        ZaolisServiceClient.ZaolisServiceClient client;
        UserDTO user;
        MainMenuViewModel mainMenuViewModel;
        public Settings(Grid mainGrid, ZaolisServiceClient.ZaolisServiceClient client, UserDTO user)
        {
            InitializeComponent();
            this.client = client;
            this.user = user;
            grid = mainGrid;
            mainMenuViewModel = new MainMenuViewModel(user);
            this.DataContext = mainMenuViewModel;
        }

        public event ClosingDelegate Closing;
        public event MessageDelegate OpenMsg;

        public void Close()
        {
            grid.Children.Remove(this);
        }

        public void ShowMsg(string title, string msg)
        {
            throw new NotImplementedException();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (nameTextBox.Text != null)
                    {
                        client.EditUsersName(user, nameTextBox.Text);
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
                        client.EditUsersBio(user, bioTextBox.Text);
                    }
                });
            });
        }
    }
}

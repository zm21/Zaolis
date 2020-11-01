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
    /// Interaction logic for MainMenuZaolis.xaml
    /// </summary>
    public partial class MainMenuZaolis : Window
    {
        ZaolisServiceClient.ZaolisServiceClient client;
        UserDTO loginnedUser;
        MainMenuViewModel mainMenuViewModel;
        public MainMenuZaolis(UserDTO user)
        {
            InitializeComponent();
            client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(new CallbackHandler()));
            loginnedUser = user;
            mainMenuViewModel = new MainMenuViewModel(user);
            this.DataContext = mainMenuViewModel;
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

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client.Disconnect(loginnedUser);
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainMenuViewModel.SearchUser(textBoxSearch.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserInfo usInfo = new UserInfo(); //test
            MainGrid.Children.Add(usInfo);
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

using System.Text;
using BLL;
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
using System.ServiceModel;
using ZaolisUI.ZaolisServiceReference;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ZaolisServiceClient client;
        RegisterViewModel registerModel;
        public MainWindow()
        {
            InitializeComponent();
            client = new ZaolisServiceClient();
            registerModel = new RegisterViewModel();
            SignUP.DataContext = registerModel;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void TOSignUp_Click(object sender, RoutedEventArgs e)
        {
            LOGIN.Visibility = Visibility.Hidden;
            SignUP.Visibility = Visibility.Visible;
        }

        private void TOLogin_Click(object sender, RoutedEventArgs e)
        {
            LOGIN.Visibility = Visibility.Visible;
            SignUP.Visibility = Visibility.Hidden;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if(client.IsExistsUserByLoginPassword(logTxtBox_login.Text, passwdbox.Password))
            {
                client.Connect(logTxtBox_login.Text,passwdbox.Password);
                
                MsgBox msg = new MsgBox("Succes!", "You are logged in");
                msg.Show();
            }
        }

        private void ButtonSignUP_Click(object sender, RoutedEventArgs e)
        {
            //client.RegisterUser(registerModel.Email);
            //VerificationCode verificationCode = new VerificationCode(registerModel.Email);
            //verificationCode.ShowDialog();
            //if(verificationCode.DialogResult == true)
            //{
            client.AddUser(new BLL.Models.UserDTO()
            {
                Login = registerModel.Login,
                Password = registerModel.Passwd,
                Name = registerModel.Login,
                Email = registerModel.Email,
                IsActive = false,
                Bio = "",
            });
            ShowMsg("SingUp", "SignUp Successfull");
            //}
        }
        private void ShowMsg(string title, string msg)
        {
            MsgBox msgBox = new MsgBox(title, msg);
            msgBox.Owner = this;
            msgBox.ShowDialog();
        }
    }
}

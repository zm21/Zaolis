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
            Task.Run(() => { this.client.Request(); });
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
            ForgetPasswordGrid.Visibility = Visibility.Hidden;
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { this.client.Request(); });
            string login = logTxtBox_login.Text;
            string password = passwdbox.Password;
            Task.Run(() =>
            {
                if (client.IsExistsUserByLoginPassword(login, password))
                {
                    client.Connect(login, password); //isActive change
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MainMenuZaolis mnz = new MainMenuZaolis();
                        mnz.Show();
                        this.Close();
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowMsg("Error!", "There is no user with such login");
                    });
                }
            });
        }
        
        private void ButtonSignUP_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { this.client.Request(); });
            
            Task.Run(() =>
            {
                client.RegisterUser(registerModel.Email);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    VerificationCode verificationCode = new VerificationCode(registerModel.Email);
                    verificationCode.Owner = this;
                    verificationCode.ShowDialog();
                    if (verificationCode.DialogResult == true)
                    {
                        client.AddUser(new BLL.Models.UserDTO()
                        {
                            Login = registerModel.Login,
                            Password = registerModel.Passwd,
                            Name = registerModel.Login,
                            Email = registerModel.Email,
                            IsActive = false,
                            Bio = "",
                        });
                        ShowMsg("Registration", "Registration Successfull");
                        SignUP.Visibility = Visibility.Hidden;
                        LOGIN.Visibility = Visibility.Visible;
                        logTxtBox_login.Text = registerModel.Login;
                        logTxtBox_Reg.Text = "";
                        emailTxtBox_reg.Text = "";
                        psswd_Reg.Text = "";
                        confirnReg_txtBox.Text = "";
                    }
                });
            });
        }
        private void ShowMsg(string title, string msg)
        {
            MsgBox msgBox = new MsgBox(title, msg);
            msgBox.Owner = this;
            msgBox.ShowDialog();
        }
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            lb_forgetpassword.Foreground = Brushes.DeepSkyBlue;
        }

        private void lb_forgetpassword_MouseLeave(object sender, MouseEventArgs e)
        {
            lb_forgetpassword.Foreground = Brushes.LightBlue;
        }

        private void lb_forgetpassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LOGIN.Visibility = Visibility.Hidden;
            ForgetPasswordGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { this.client.Request(); });
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var user_res = client.GetUserByLogin(ForgPassTxtBox_login.Text);
                    if (user_res != null)
                    {
                        client.ForgetPassword(user_res);
                        ForgotPasswordCode forgotPassword = new ForgotPasswordCode(ForgPassTxtBox_login.Text);
                        forgotPassword.Owner = this;
                        forgotPassword.ShowDialog();
                        if (forgotPassword.DialogResult == true)
                        {
                            ForgetPasswordGrid.Visibility = Visibility.Hidden;
                            NewPasswordGrid.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(() => { ShowMsg("Error", "Error! \nWindow was closed!"); });
                        }
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() => { ShowMsg("Error", "Such user was not found"); });
                    }
                });
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { this.client.Request(); });
            string newpass= newPass_txtBox.Password;
            string confirmnewpass= confirmNewPass_txtBox.Password;
            string forgpasslogin = ForgPassTxtBox_login.Text;
            Task.Run(() => 
            {
                Application.Current.Dispatcher.Invoke(() => 
                {
                    if (newpass == confirmnewpass)
                    {
                        if (newpass.Length >= 8)
                        {
                            var res = client.GetUserByLogin(forgpasslogin);
                            client.EditUsersPassword(res, newpass);
                            NewPasswordGrid.Visibility = Visibility.Hidden;
                            LOGIN.Visibility = Visibility.Visible;
                            logTxtBox_login.Text = forgpasslogin;
                            ForgPassTxtBox_login.Text = "";
                            newPass_txtBox.Password = "";
                            confirmNewPass_txtBox.Password = "";
                        }
                    }
                });
                
            });
            
        }

    }
}

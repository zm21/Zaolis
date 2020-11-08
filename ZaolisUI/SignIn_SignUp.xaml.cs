using System.Text;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
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
using ZaolisUI.ZaolisServiceClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using BLL.Models;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure.Design;

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SignInUpWindow : Window
    {
        private static string remembered_path = "rememberme.dat";

        ZaolisServiceClient.ZaolisServiceClient client;
        RegisterViewModel registerModel;
        ProgressBar pgLoading;
        CallbackHandler handler = new CallbackHandler();
        ObservableCollection<UserDTO> logginedUsers = new ObservableCollection<UserDTO>();
        public SignInUpWindow()
        {
            InitializeComponent();
            rememberMe.IsChecked = false;
            client = new ZaolisServiceClient.ZaolisServiceClient(new InstanceContext(handler));
            if(File.Exists(remembered_path))
            {
                logginedUsers = Load();
                if (logginedUsers.Count > 0)
                {
                    MainMenuZaolis mnz = new MainMenuZaolis(logginedUsers, client);
                    mnz.Show();
                    this.Close();
                }
            }
            registerModel = new RegisterViewModel(client);
            SignUP.DataContext = registerModel;
            pgLoading = loginProgressBar;
            buttonLogin.Content = "LOGIN";
            Task.Run(() => { this.client.Request(); });
        }

        public SignInUpWindow(ObservableCollection<UserDTO> logginedUsers, ZaolisServiceClient.ZaolisServiceClient client)
        {
            InitializeComponent();
            this.logginedUsers = logginedUsers;
            this.client = client;
            registerModel = new RegisterViewModel(client);
            pgLoading = loginProgressBar;
            buttonLogin.Content = "LOGIN";
            Task.Run(() => { this.client.Request(); });
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        public static ObservableCollection<UserDTO> Load()
        {
            using (FileStream fs = new FileStream(remembered_path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var temp = (ObservableCollection<UserDTO>)formatter.Deserialize(fs);
                return temp;
            }
        }

        public static void Save(ObservableCollection<UserDTO> collection)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(remembered_path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, collection);
            }
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
            buttonLogin.Content = pgLoading;
            buttonLogin.IsEnabled = false;
            Task.Run(() =>
            {
                if (client.IsExistsUserByLoginPassword(login, password))
                {
                    client.Connect(login, password); //isActive change
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UserDTO user = new UserDTO();
                        user = client.GetUserByLogin(logTxtBox_login.Text);
                        logginedUsers.Add(user);
                        if (rememberMe.IsChecked == true)
                        {
                            Save(logginedUsers);
                        }
                        MainMenuZaolis mnz = new MainMenuZaolis(logginedUsers, client);
                        mnz.Show();
                        this.Close();
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        buttonLogin.Content = "LOGIN";
                        buttonLogin.IsEnabled = true;
                        ShowMsg("Error!", "There is no user with such login or password");
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
                    VerificationCode verificationCode = new VerificationCode(registerModel.Email,client);
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
                            LastActive=DateTime.Now
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
            string newpass = newPass_txtBox.Password;
            string confirmnewpass = confirmNewPass_txtBox.Password;
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
                    else
                    {
                        ShowMsg("Error", "Password don't match");
                    }
                });

            });

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}

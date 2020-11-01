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
    /// Interaction logic for ForgotPasswordCode.xaml
    /// </summary>
    public partial class ForgotPasswordCode : Window
    {
        ZaolisServiceClient.ZaolisServiceClient client;
        CallbackHandler handler;
        string login;
        public ForgotPasswordCode(string login)
        {
            InitializeComponent();
            this.login = login;
            client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(handler));
        }

        private void Num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_VereficationCode.Text == client.GetVerificationCodeFromEmail(client.GetUserByLogin(login).Email).ToString())
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MsgBox msg = new MsgBox("Error!", "Wrong code");
                msg.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult != true)
                this.DialogResult = false;
        }
    }
}

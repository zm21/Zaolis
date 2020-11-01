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
    /// Interaction logic for VerificationCode.xaml
    /// </summary>
    public partial class VerificationCode : Window
    {
        CallbackHandler handler;
        ZaolisServiceClient.ZaolisServiceClient client;
        string email;
        public VerificationCode(string email)
        {
            InitializeComponent();
            this.email = email;
            client = new ZaolisServiceClient.ZaolisServiceClient(new System.ServiceModel.InstanceContext(handler));
        }

        private void Num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_VereficationCode.Text == client.GetCodeFromEmail(email).ToString())
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
    }
}

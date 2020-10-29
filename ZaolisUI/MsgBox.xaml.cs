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

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for MsgBox.xaml
    /// </summary>
    public partial class MsgBox : Window
    {
        public MsgBox(string title, string message)
        {
            InitializeComponent();
            MsgTitle.Content = title;
            Message.Text = message;

        }
        private void BtnMsgBoxClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}

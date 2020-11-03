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

namespace ZaolisUI
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl, IChildWindow
    {
        Grid grid;
        public UserInfo(Grid mainGrid)
        {
            InitializeComponent();
            grid = mainGrid;
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
    }
}

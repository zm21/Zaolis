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
    /// Interaction logic for GroupInfo.xaml
    /// </summary>
    public partial class GroupInfo : UserControl
    {
        DockPanel parent;

        public GroupInfo(DockPanel parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        public void Close()
        {
            parent.Children.Remove(this);
        }
    }
}

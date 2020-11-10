using System.Windows.Controls;

namespace ZaolisUI
{
    public delegate void ClosingDelegate();

    public delegate void MessageDelegate(string title, string msg);

    public interface IChildWindow
    {
        event ClosingDelegate Closing;
        event MessageDelegate OpenMsg;

        void Close();

        void ShowMsg(string title, string msg);
    }
}

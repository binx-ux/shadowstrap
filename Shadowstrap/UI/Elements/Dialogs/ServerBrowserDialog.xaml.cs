using Shadowstrap.UI.ViewModels;

namespace Shadowstrap.UI.Elements.Dialogs
{
    public partial class ServerBrowserDialog
    {
        public ServerBrowserDialog()
        {
            DataContext = new ServerBrowserViewModel();
            InitializeComponent();
        }
    }
}

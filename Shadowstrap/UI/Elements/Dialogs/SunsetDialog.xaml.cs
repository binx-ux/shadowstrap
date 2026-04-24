using System.Windows;

namespace Shadowstrap.UI.Elements.Dialogs
{
    /// <summary>
    /// Shutdown notice shown on every user-facing launch.
    /// The project is discontinued — users are prompted to uninstall.
    /// </summary>
    public partial class SunsetDialog
    {
        /// <summary>True when the user clicked "Uninstall Now".</summary>
        public bool UninstallNow { get; private set; } = false;

        public SunsetDialog()
        {
            InitializeComponent();
        }

        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {
            UninstallNow = true;
            Close();
        }
    }
}

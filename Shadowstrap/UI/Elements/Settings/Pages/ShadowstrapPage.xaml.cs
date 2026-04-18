using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shadowstrap.UI.ViewModels.Settings;

namespace Shadowstrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for ShadowstrapPage.xaml
    /// </summary>
    public partial class ShadowstrapPage
    {
        public ShadowstrapPage()
        {
            DataContext = new ShadowstrapViewModel();
            InitializeComponent();
        }
    }
}

using IoToaster_App.Views;
using Xamarin.Forms;

namespace IoToaster_App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CookingStatusPage), typeof(CookingStatusPage));
        }

       
    }
}

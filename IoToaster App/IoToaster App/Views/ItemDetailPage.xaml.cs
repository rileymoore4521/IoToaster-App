using IoToaster_App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace IoToaster_App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
using IoToaster_App.Models;
using IoToaster_App.Services;
using IoToaster_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IoToaster_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IoToasterHomePage : ContentPage
    {
        public IoToasterHomePage()
        {
            InitializeComponent();
            

        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var cookingPreset = ((ListView)sender).SelectedItem as CookingPreset;
            if (cookingPreset == null)
                return;

            await DisplayAlert("Cooking Preset Selected", cookingPreset.Name,"Ok");
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var cookingPreset = ((MenuItem)sender).BindingContext as CookingPreset;
            if (cookingPreset == null)
                return;

            await DisplayAlert("Cooking Started on:", cookingPreset.Name, "Ok");

        }

        private void MenuItem_Clicked_1(object sender, EventArgs e)
        {

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var vm = (IoToasterViewModel)BindingContext;
            if (vm.CookingPresets.Count == 0)
                await vm.RefreshCommand.ExecuteAsync();
        }
    }
}
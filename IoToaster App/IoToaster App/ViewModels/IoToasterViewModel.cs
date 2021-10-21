using IoToaster_App.Models;
using IoToaster_App.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace IoToaster_App.ViewModels
{
    class IoToasterViewModel : ViewModelBase
    {
        public ObservableRangeCollection<CookingPreset> CookingPresets { get; set; }

       

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand AddCommand { get; }

        public AsyncCommand<CookingPreset> RemoveCommand { get; }

        public AsyncCommand<CookingPreset> StartCookingCommand { get; }
        public IoToasterViewModel()
        {
            Title = "IoToaster App";

            CookingPresets = new ObservableRangeCollection<CookingPreset>();
        


            RefreshCommand = new AsyncCommand(Refresh);
            StartCookingCommand = new AsyncCommand<CookingPreset>(StartCooking);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<CookingPreset>(Remove);
        }

        async Task StartCooking(CookingPreset cookingPreset)
        {
            if (cookingPreset == null)
                return;
            
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Started Cooking", cookingPreset.Name, "OK");

        }
        CookingPreset selectedCookingPreset;

        public CookingPreset SelectedCookingPreset
        {
            get => selectedCookingPreset;
            set
            {
                if(value != null)
                {
                    Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Selected", value.Name, "OK");
                    value = null;
                }

                selectedCookingPreset = value;
                OnPropertyChanged();
            }
        }
        async Task Add()
        {
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "The name the preset will be saved under");
            var toastDuration = await App.Current.MainPage.DisplayPromptAsync("Toast Duration", "The duration the item will be toasted");
            var temperature = await App.Current.MainPage.DisplayPromptAsync("Temperature", "The Temperature to cook the item at");
            
            await CookingPresetService.AddCookingPreset(name, Convert.ToDouble(toastDuration), Convert.ToInt32(temperature));
            await Refresh();
        }
        async Task Remove(CookingPreset cookingPreset)
        {
            await CookingPresetService.RemoveCookingPreset(cookingPreset.Id);
            await Refresh();
        }
        async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);

            CookingPresets.Clear();

            var cookingPresets = await CookingPresetService.GetCookingPresets();

            CookingPresets.AddRange(cookingPresets);

            IsBusy = false;
        }
       
    }
}

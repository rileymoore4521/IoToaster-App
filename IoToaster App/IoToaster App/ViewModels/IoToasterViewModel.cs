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

        public AsyncCommand<CookingPreset> EditCommand { get; }
        public AsyncCommand<CookingPreset> StartCookingCommand { get; }
        public IoToasterViewModel()
        {
            Title = "IoToaster App";

            CookingPresets = new ObservableRangeCollection<CookingPreset>();



            RefreshCommand = new AsyncCommand(Refresh);
            StartCookingCommand = new AsyncCommand<CookingPreset>(StartCooking);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<CookingPreset>(Remove);
            EditCommand = new AsyncCommand<CookingPreset>(Edit);
            
        }
        private Color cookingButtonBgColor = Color.Green;
        public Color CookingButtonBgColor
        {
            get => cookingButtonBgColor;
            set
            {
                if (value == cookingButtonBgColor)
                    return;

                cookingButtonBgColor = value;
                OnPropertyChanged();

            }
        }
        private string cookingButtonText = "Start Cooking";
        public string CookingButtonText
        {
            get => cookingButtonText;
            set
            {
                if (value == cookingButtonText)
                    return;

                cookingButtonText = value;
                OnPropertyChanged();

            }
        }



        bool isCooking = false;
        
        async Task StartCooking(CookingPreset cookingPreset)
        {
            
            if(cookingPreset != null && !isCooking)
            {
                isCooking = true;
                CookingButtonBgColor = Color.Red;
                CookingButtonText = "Stop Cooking";
                await InternetCookingPresetService.UpdateCookingStatus(cookingPreset, false);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Started Cooking", cookingPreset.Name, "OK");
            }else if (cookingPreset != null && isCooking)
            {
                isCooking = false;
                CookingButtonBgColor = Color.Green;
                CookingButtonText = "Start Cooking";
                await InternetCookingPresetService.UpdateCookingStatus(cookingPreset, true);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Stopped Cooking", cookingPreset.Name, "OK");
            }
            
            

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
            
            await InternetCookingPresetService.AddCookingPreset(name, Convert.ToInt32(toastDuration), Convert.ToInt32(temperature));
            await Refresh();
        }
        async Task Remove(CookingPreset cookingPreset)
        {
            await InternetCookingPresetService.RemoveCookingPreset(cookingPreset._id);
            await Refresh();
        }
        async Task Edit(CookingPreset cookingPreset)
        {
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "The name the preset will be saved under");
            var toastDuration = await App.Current.MainPage.DisplayPromptAsync("Toast Duration", "The duration the item will be toasted");
            var temperature = await App.Current.MainPage.DisplayPromptAsync("Temperature", "The Temperature to cook the item at");
            cookingPreset.Name = name;
            cookingPreset.ToastDuration = Convert.ToInt32(toastDuration);
            cookingPreset.Temperature = Convert.ToInt32(temperature);
            await InternetCookingPresetService.EditCookingPreset(cookingPreset._id,cookingPreset);
            await Refresh();
        }
        async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);

            CookingPresets.Clear();

            var cookingPresets = await InternetCookingPresetService.GetCookingPresets();

            CookingPresets.AddRange(cookingPresets);
           
            IsBusy = false;
        }
       
    }
}

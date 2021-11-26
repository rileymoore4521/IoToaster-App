using IoToaster_App.Models;
using IoToaster_App.Services;
using IoToaster_App.Views;
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
        
        async Task StartCooking(CookingPreset cookingPreset)
        {
        
            await InternetCookingPresetService.UpdateCookingStatus(cookingPreset, false);
            var route = $"{nameof(CookingStatusPage)}?CookingPresetName={cookingPreset.Name}&Temperature={cookingPreset.Temperature}&Timeremaining={cookingPreset.ToastDuration}";
            await AppShell.Current.GoToAsync(route);

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
            int toastTime = 0;
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "The name the preset will be saved under");
            var toastDuration = await App.Current.MainPage.DisplayPromptAsync("Toast Duration", "The duration the item will be toasted in seconds");
            while(int.TryParse(toastDuration, out toastTime) == false || toastTime < 0 || toastTime > 180)
            {
                toastDuration = await App.Current.MainPage.DisplayPromptAsync("Toast Duration", "The toasting duration you inputted is not valid.\nPlease input a new toast duration in seconds between 0 and 180.");
            }
            await InternetCookingPresetService.AddCookingPreset(name, toastTime, 0);
            await Refresh();
        }
        async Task Remove(CookingPreset cookingPreset)
        {
            await InternetCookingPresetService.RemoveCookingPreset(cookingPreset._id);
            await Refresh();
        }
        async Task Edit(CookingPreset cookingPreset)
        {
            int toastTime = 0;
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "The name the preset will be saved under");
            var toastDuration = await App.Current.MainPage.DisplayPromptAsync("Toast Duration", "The duration the item will be toasted");
            while (int.TryParse(toastDuration, out toastTime) == false || toastTime < 0 || toastTime > 180)
            {
                toastDuration = await App.Current.MainPage.DisplayPromptAsync("Toast Duration", "The toasting duration you inputted is not valid.\nPlease input a new toast duration in seconds between 0 and 180.");
            }
            cookingPreset.Name = name;
            cookingPreset.ToastDuration = Convert.ToInt32(toastDuration);
            cookingPreset.Temperature = 0;
            await InternetCookingPresetService.EditCookingPreset(cookingPreset._id,cookingPreset);
            await Refresh();
        }
        async Task Refresh()
        {
            IsBusy = true;

            CookingPresets.Clear();

            var cookingPresets = await InternetCookingPresetService.GetCookingPresets();

            CookingPresets.AddRange(cookingPresets);
           
            IsBusy = false;
        }
       
    }
}

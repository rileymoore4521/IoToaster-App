using IoToaster_App.Models;
using IoToaster_App.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IoToaster_App.ViewModels
{
 
    [QueryProperty(nameof(CookingPresetName), nameof(CookingPresetName))]
    [QueryProperty(nameof(Temperature), nameof(Temperature))]
    [QueryProperty(nameof(Timeremaining), nameof(Timeremaining))]
    public class CookingStatusPageViewModel : ViewModelBase
    {

        
       
      
        public ObservableRangeCollection<CookingPreset> cookingPresets { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand StopCookingCommand { get; }
        public CookingStats cookingStats = new CookingStats();
        
        public CookingStatusPageViewModel()
        {
            Statusinfo = new ObservableRangeCollection<StatusInfo>();
            RefreshCommand = new AsyncCommand(Refresh);
            StopCookingCommand = new AsyncCommand(StopCooking);
            cookingPresets = new ObservableRangeCollection<CookingPreset>();

          
            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
             {
                 Device.BeginInvokeOnMainThread(async () =>
                 {
                    await Refresh();
                 });

                 return true;
             });
            

        }
        public string cookingPresetName = "";
        public string CookingPresetName
        {
            get => cookingPresetName;
            set
            {
                if (value == cookingPresetName)
                    return;

                cookingPresetName = value;
                OnPropertyChanged();
            }
        }
        async Task getCookingPreset(string _id)
        {
            await InternetCookingPresetService.GetCookingPreset(_id);
        }
        public class StatusInfo
        {
            public string status;
        }
        
        public static ObservableRangeCollection<StatusInfo> Statusinfo { get; set; }
        public string Temperature
        {
            get => cookingStats.temperature;
            set
            {
                if (value == cookingStats.temperature)
                    return;
                cookingStats.temperature = value;
                OnPropertyChanged();
            }
        }
        public string Timestamp
        {
            get => cookingStats.timestamp;
            set
            {
                if (value == cookingStats.timestamp)
                    return;
                cookingStats.timestamp = value;
                OnPropertyChanged();
            }
        }
        public string Timeremaining
        {
            get => cookingStats.timeremaining;
            set
            {
                if (value == cookingStats.timeremaining)
                    return;
                cookingStats.timeremaining = value;
                OnPropertyChanged();
            }
        }
        public string Cookingstatus
        {
            get => cookingStats.cookingstatus;
            set
            {
                if (value == cookingStats.cookingstatus)
                    return;
                 cookingStats.cookingstatus = value;
                OnPropertyChanged();
            }
        }
        async Task StopCooking()
        {
            await InternetCookingPresetService.StopCooking(true);
            await AppShell.Current.GoToAsync("..");
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Stopped Cooking", CookingPresetName, "OK");

        }
        async Task getCookingStatus()
        {
            var Stats = await InternetCookingPresetService.getStatusInfo();
            Statusinfo.AddRange(Stats);
            int numOfItems = 0;
            numOfItems = Statusinfo.Count;
            string testString = Statusinfo[0].status;
            string[] values = testString.Split(',');

            Timestamp = values[0];
            Temperature = values[1];
            Timeremaining = values[2];
            Cookingstatus = values[3].ToLower();

             
        }
        
        async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);

            Statusinfo.Clear();

            await getCookingStatus();

            IsBusy = false;
        }
    }
}

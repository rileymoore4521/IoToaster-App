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
    public class CookingStatusPageViewModel : ViewModelBase
    {
        public AsyncCommand RefreshCommand { get; }
        public CookingStats cookingStats = new CookingStats();
        
        public CookingStatusPageViewModel()
        {
            Statusinfo = new ObservableRangeCollection<StatusInfo>();
            RefreshCommand = new AsyncCommand(Refresh);
            
            
            cookingStats.error = "null";
            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
             {
                 Device.BeginInvokeOnMainThread(async () =>
                 {
                    await Refresh();
                 });

                 return true;
             });

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
        public string Error
        {
            get => cookingStats.error;
            set
            {
                if (value == cookingStats.error)
                    return;
                 cookingStats.error = value;
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error Code:", value, "OK");
                OnPropertyChanged();
            }
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
            Error = values[3];

             
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

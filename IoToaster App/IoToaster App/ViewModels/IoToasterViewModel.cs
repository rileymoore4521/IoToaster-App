using IoToaster_App.Models;
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

        public ObservableRangeCollection<Grouping<string,CookingPreset>> CookingPresetsGroups { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public IoToasterViewModel()
        {
            Title = "IoToaster App";

            CookingPresets = new ObservableRangeCollection<CookingPreset>();
            CookingPresetsGroups = new ObservableRangeCollection<Grouping<string, CookingPreset>>();

            CookingPresets.Add(new CookingPreset { Name = "Everything Bagel", Temperature = 50, ToastDuration = 1 });
            CookingPresets.Add(new CookingPreset { Name = "Plain Bagel", Temperature = 50, ToastDuration = 1.5 });
            CookingPresets.Add(new CookingPreset { Name = "English Muffin", Temperature = 50, ToastDuration = 2 });

            CookingPresetsGroups.Add(new Grouping<string, CookingPreset>("English Muffin", new[] {CookingPresets.Last()}));
            CookingPresetsGroups.Add(new Grouping<string, CookingPreset>("Bagel", CookingPresets.Take(2)));

            RefreshCommand = new AsyncCommand(Refresh);

        }

  
        async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);
            IsBusy = false;
        }
        public ICommand IncreaseCount { get; }
        int count = 0;
        string temperatureDisplay = "Temperature 0 F";
        string timeRemainingDisplay = "Time Remaining 0 seconds";
        string cookingStateDisplay = "Stop Cooking";
        Color cookingStateDisplayBackgroundColor = Color.Red;
        string cookingPresetDisplay = "Select a Cooking Preset!";
        string selectedCookingPresetName = "Bagel";
        public string TemperatureDisplay
        {
            get => temperatureDisplay;
            set
            {
                if (value == temperatureDisplay)
                    return;

                temperatureDisplay = value;
                OnPropertyChanged();
            }
        }
        public string TimeRemainingDisplay
        {
            get => timeRemainingDisplay;
            set
            {
                if (value == timeRemainingDisplay)
                    return;

                timeRemainingDisplay = value;
                OnPropertyChanged();
            }
        }
        public string CookingStateDisplay
        {
            get => cookingStateDisplay;
            set
            {
                if (value == cookingStateDisplay)
                    return;
                cookingStateDisplay = value;
                OnPropertyChanged();
            }
        }
        public Color CookingStateDisplayBackgroundColor
        {
            get => cookingStateDisplayBackgroundColor;
            set
            {
                if (value == cookingStateDisplayBackgroundColor)
                    return;
                cookingStateDisplayBackgroundColor = value;
                OnPropertyChanged();
            }
        }
        public string CookingPresetDisplay
        {
            get => cookingPresetDisplay;
            set
            {
                if (value == cookingPresetDisplay)
                    return;

                cookingPresetDisplay = value;
                OnPropertyChanged();
            }
        }
        void OnIncrease()
        {
            if (CookingStateDisplay.Equals("Start Cooking"))
            {
                count++;
                TemperatureDisplay = $"Temperature {count} F";
                TimeRemainingDisplay = $"Time Remaining {count} seconds";
                CookingStateDisplay = "Stop Cooking";
                CookingPresetDisplay = $"Cooking: {selectedCookingPresetName} ";
                CookingStateDisplayBackgroundColor = Color.Red;

                //add in function call to PI to stop toaster
            }
            else
            {
                count++;
                CookingStateDisplayBackgroundColor = Color.Green;
                CookingStateDisplay = "Start Cooking";
                CookingPresetDisplay = $"Cooking Preset selected:{selectedCookingPresetName}";
                //add in function call to PI to stop toaster.
            }
        }
    }
}

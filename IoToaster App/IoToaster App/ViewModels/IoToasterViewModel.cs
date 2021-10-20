using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IoToaster_App.ViewModels
{
    class IoToasterViewModel : BindableObject
    {
        public IoToasterViewModel()
        {
            IncreaseCount = new Command(OnIncrease);
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

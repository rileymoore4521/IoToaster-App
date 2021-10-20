using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            BindingContext = this;
           
        }
        int count = 0;
        string temperatureDisplay = "Temperature 0 F";
        string timeRemainingDisplay = "Time Remaining 0 seconds";
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
        private void StartStopButton_Clicked(object sender, EventArgs e)
        {
            if (StartStopButton.Text.Equals("Start Cooking"))
            {
                count++;
                TemperatureDisplay = $"Temperature {count} F";
                TimeRemainingDisplay = $"Time Remaining {count} seconds";
                StartStopButton.Text = "Stop Cooking";
                StartStopButton.BackgroundColor = Color.Red;
              
                //add in function call to PI to stop toaster
            }
            else
            {
                count++;
                StartStopButton.BackgroundColor = Color.Green;
                StartStopButton.Text = "Start Cooking";
               
                //add in function call to PI to stop toaster.
            }


        }
    }
}
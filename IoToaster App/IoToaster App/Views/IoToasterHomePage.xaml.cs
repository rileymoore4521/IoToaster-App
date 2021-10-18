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
        }

        private void StartStopButton_Clicked(object sender, EventArgs e)
        {
            if (StartStopButton.Text.Equals("Start Cooking"))
            {
                StartStopButton.Text = "Stop Cooking";
                StartStopButton.BackgroundColor = Color.Red;
                //add in function call to PI to stop toaster
            }
            else
            {
                StartStopButton.BackgroundColor = Color.Green;
                StartStopButton.Text = "Start Cooking";
                //add in function call to PI to stop toaster.
            }


        }
    }
}
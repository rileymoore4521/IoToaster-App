using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace IoToaster_App.Services
{
    public static class InternetCookingPresetService
    {

        static string BaseUrl = "https://zbechhoefer-052e.restdb.io/rest/data%22";

        static HttpClient client;

        static InternetCookingPresetService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }
       

    }
}

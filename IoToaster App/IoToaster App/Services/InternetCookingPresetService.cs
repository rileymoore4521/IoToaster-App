using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using IoToaster_App.Models;

using Newtonsoft.Json;


namespace IoToaster_App.Services
{



    public static class InternetCookingPresetService
    {

        static string BaseUrl = "https://zbechhoefer-052e.restdb.io/rest/";

        static HttpClient client;
        static InternetCookingPresetService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)

            };
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            client.DefaultRequestHeaders.Add("x-apikey", "a0fed473e5a6170c2a06583f0030cf12247f6");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
          
        }
        public static async Task<IEnumerable<CookingPreset>> GetCookingPresets()
        {
            var json = await client.GetStringAsync("cookingpresets");
            var cookingPresets = JsonConvert.DeserializeObject<IEnumerable<CookingPreset>>(json);
            return cookingPresets;
        }

        public static async Task AddCookingPreset(string name, int toastDuration, int temperature)
        {
            
            var cookingPreset = new CookingPreset
            {
                Name = name,
                ToastDuration = toastDuration,
                Temperature = temperature

            };
            var json = JsonConvert.SerializeObject(cookingPreset);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync("cookingpresets", content);
           
            if(!response.IsSuccessStatusCode)
            {

            }
        }
        public static async Task RemoveCookingPreset(string id)
        {
            var response = await client.DeleteAsync($"cookingpresets/{id}");
            if(!response.IsSuccessStatusCode)
            {

            }

        }

    }
}

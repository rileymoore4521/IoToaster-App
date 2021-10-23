using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using IoToaster_App.Models;

using Newtonsoft.Json;
using MvvmHelpers;

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
        public static async Task EditCookingPreset(string id, CookingPreset cookingPreset)
        {
            var json = JsonConvert.SerializeObject(cookingPreset);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"cookingpresets/{id}", content);
            if (!response.IsSuccessStatusCode)
            {

            }

        }
        public class DataInfo
        {
            public string _id;
            public string phone_instr;
            
        };
        public static ObservableRangeCollection<DataInfo> Datainfo { get; set; }
        public static async Task<IEnumerable<DataInfo>> GetDataInfo()
        {
            var json = await client.GetStringAsync("data");
            var datainfo = JsonConvert.DeserializeObject<IEnumerable<DataInfo>>(json);
            return datainfo;
        }
        
        public static async Task UpdateCookingStatus(CookingPreset cookingPreset, bool stopCooking)
        {
            DateTime localDate = DateTime.Now;
            TimeSpan localTime = localDate.TimeOfDay;
            localTime = new TimeSpan(localTime.Hours, localTime.Minutes, localTime.Seconds);
            string updateStr = localTime.ToString();
            Datainfo = new ObservableRangeCollection<DataInfo>();
            var dataInfo = await GetDataInfo();
            Datainfo.AddRange(dataInfo);
            
            string dataId = "";
            int numOfItems = Datainfo.Count;
            if (Datainfo != null && numOfItems != 0)
            {
                dataId = Datainfo[0]._id;

                if (stopCooking == false)
                {
                    updateStr = updateStr + $",{cookingPreset.ToastDuration}";
                    Datainfo[0].phone_instr = updateStr;
                    var json = JsonConvert.SerializeObject(Datainfo[0]);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var method = new HttpMethod("PATCH");
                    var request = new HttpRequestMessage(method, BaseUrl + $"data/{dataId}")
                    {
                        Content = content
                    };
                    var response = new HttpResponseMessage();

                    try
                    {
                        response = await client.SendAsync(request);
                    }
                    catch (TaskCanceledException e)
                    {

                    }
                    if (!response.IsSuccessStatusCode)
                    {

                    }
                }
                else if (stopCooking == true)
                {
                    updateStr = updateStr + ",Stop Cooking";
                    Datainfo[0].phone_instr = updateStr;
                    var json = JsonConvert.SerializeObject(Datainfo[0]);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var method = new HttpMethod("PATCH");
                    var request = new HttpRequestMessage(method, BaseUrl + $"data/{dataId}")
                    {
                        Content = content
                    };
                    var response = new HttpResponseMessage();

                    try
                    {
                        response = await client.SendAsync(request);
                    }
                    catch (TaskCanceledException e)
                    {

                    }
                    if (!response.IsSuccessStatusCode)
                    {

                    }

                }
            }


        }

    }
}

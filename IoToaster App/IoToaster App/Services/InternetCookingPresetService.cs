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
using static IoToaster_App.ViewModels.CookingStatusPageViewModel;

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

            if (!response.IsSuccessStatusCode)
            {

            }
        }
        public static async Task RemoveCookingPreset(string id)
        {
            var response = await client.DeleteAsync($"cookingpresets/{id}");
            if (!response.IsSuccessStatusCode)
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
        public class InstructionInfo
        {
            public string _id;
            public string phone_instr;

        };
        public static ObservableRangeCollection<InstructionInfo> instructionInfo { get; set; }
        public static async Task<IEnumerable<InstructionInfo>> GetInstructionInfo()
        {
            var json = await client.GetStringAsync("data");
            var InstructionInfo = JsonConvert.DeserializeObject<IEnumerable<InstructionInfo>>(json);
            return InstructionInfo;
        }

        public static async Task UpdateCookingStatus(CookingPreset cookingPreset, bool stopCooking)
        {
            DateTime localDate = DateTime.Now;
            TimeSpan localTime = localDate.TimeOfDay;
            localTime = new TimeSpan(localTime.Hours, localTime.Minutes, localTime.Seconds);
            string updateStr = localTime.ToString();
            instructionInfo = new ObservableRangeCollection<InstructionInfo>();
            var Instructioninfo = await GetInstructionInfo();
            instructionInfo.AddRange(Instructioninfo);

            string dataId = "";
            int numOfItems = instructionInfo.Count;
            if (instructionInfo != null && numOfItems != 0)
            {
                dataId = instructionInfo[0]._id;

                if (stopCooking == false)
                {
                    updateStr = updateStr + $",{cookingPreset.ToastDuration}";
                    instructionInfo[0].phone_instr = updateStr;
                    var json = JsonConvert.SerializeObject(instructionInfo[0]);
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
                    instructionInfo[0].phone_instr = updateStr;
                    var json = JsonConvert.SerializeObject(instructionInfo[0]);
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
        
        public static async Task<IEnumerable<StatusInfo>> getStatusInfo()
        {
            var json = await client.GetStringAsync("data");
            var statusInfo = JsonConvert.DeserializeObject<IEnumerable<StatusInfo>>(json);
            return statusInfo;
        }
        
    }
}

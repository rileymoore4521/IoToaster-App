using IoToaster_App.Services;
using IoToaster_App.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IoToaster_App.Services
{
    public static class CookingPresetService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            // Get an absolute path to the database file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<CookingPreset>();
        }

        public static async Task AddCookingPreset(string name, double toastDuration, int temperature)
        {
            await Init();
            var cookingPreset = new CookingPreset
            { 
                Name = name,
                ToastDuration = toastDuration,
                Temperature = temperature

            };

            var id = await db.InsertAsync(cookingPreset);
        }

        public static async Task RemoveCookingPreset(int id)
        {
            await Init();

            await db.DeleteAsync<CookingPreset>(id);

        }

        public static async Task<IEnumerable<CookingPreset>> GetCookingPresets()
        {
            await Init();

            var cookingPreset = await db.Table<CookingPreset>().ToListAsync();
            return cookingPreset;
        }
    }
}

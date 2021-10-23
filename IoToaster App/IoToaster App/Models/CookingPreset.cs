using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoToaster_App.Models
{
   public class CookingPreset
    {
        [PrimaryKey, AutoIncrement]
        public string _id { get; set; }
        public string Name { get; set; }
        public int ToastDuration { get; set; }
        public int Temperature { get; set; }
    }
}

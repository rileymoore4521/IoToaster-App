using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoToaster_App.Models
{
   public class CookingPreset
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double ToastDuration { get; set; }
        public int Temperature { get; set; }
    }
}

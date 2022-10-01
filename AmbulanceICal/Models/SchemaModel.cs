using System;
namespace AmbulanceICal.Models
{
    public class SchemaModel
    {
        public int Week { get; set; }
        public int Day { get; set; }
        public KeyValuePair<int, string> Shift { get; set; }
        public string WorkHours { get; set; }
        public string Team { get; set; }
    }
}


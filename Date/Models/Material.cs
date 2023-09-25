using System;

namespace MVP.Date.Models
{
    public class Material
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }

        public string countName { get; set; }
        public int planCount { get; set; }
        public int factCount { get; set; }

        public int planPrice { get; set; }
        public int factPrice {get;set;}

        public string status { get; set; }

        public DateTime planPayDate { get; set; }
        public DateTime factPayDate { get; set; }

        public DateTime planDeliveryDate { get; set; }
        public DateTime factDeliveryDate { get; set; }

        public int taskId { get; set; }
        public string taskName { get; set; }

        public int projId { get; set; }

        public string provider { get; set; }

    }
}

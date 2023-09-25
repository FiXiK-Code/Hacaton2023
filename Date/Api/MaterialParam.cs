using System;
using System.Collections.Generic;

namespace MVP.Date.Api
{
    public class MaterialParam
    {
        public string filterDate { get; set; } = null;
        public string[] filterCategory { get; set; } = null;
        public string[] filterProvider { get; set; } = null;
        public string[] filterTask { get; set; } = null;
        public string token { get; set; }

        public int id { get; set; } = 0;
        public string name { get; set; } = null;
        public string category { get; set; } = null;

        public string countName { get; set; } = null;
        public int planCount { get; set; } = 0;
        public int factCount { get; set; } = 0;

        public int planPrice { get; set; } = 0;
        public int factPrice { get; set; } = 0;

        public string status { get; set; } = null;

        public string planPayDate { get; set; } = null;
        public string factPayDate { get; set; } = null;

        public string planDeliveryDate { get; set; } = null;
        public string factDeliveryDate { get; set; } = null;


        public int taskId { get; set; } = 0;
        public string taskName { get; set; } = null;

        public int projId { get; set; } = 0;

        public string provider { get; set; } = null;
    }
}

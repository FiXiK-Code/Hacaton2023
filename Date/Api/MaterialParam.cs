using System;
using System.Collections.Generic;

namespace MVP.Date.Api
{
    public class MaterialParam
    {
        public string filter { get; set; }
        public string token { get; set; }

        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }

        public string countName { get; set; }
        public int planCount { get; set; }
        public int factCount { get; set; }

        public int planPrice { get; set; }
        public int factPrice { get; set; }

        public string status { get; set; }

        public string planPayDate { get; set; }
        public string factPayDate { get; set; }

        public int taskId { get; set; }
        public string taskName { get; set; }

        public int projId { get; set; }

        public string provider { get; set; }
    }
}

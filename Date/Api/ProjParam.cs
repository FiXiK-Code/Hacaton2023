using System;
using System.Collections.Generic;

namespace MVP.Date.Api
{
    public class ProjParam
    {
        public string filter { get; set; }
        public string token { get; set; }

        public int id { get; set; }
        public string name { get; set; }
        public string adr { get; set; }

        public string supervisor { get; set; }

        public DateTime planStartDate { get; set; }
        public DateTime factStartDate { get; set; }

        public DateTime planFinishDate { get; set; }
        public DateTime factFinishDate { get; set; }

        public int planWorkPrice { get; set; }
        public int factWorkPrice { get; set; }

        public int planMaterialPrice { get; set; }
        public int factMaterialPrice { get; set; }
    }
}

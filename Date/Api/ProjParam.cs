using System;
using System.Collections.Generic;

namespace MVP.Date.Api
{
    public class ProjParam
    {
        public string filter { get; set; } = null;
        public string token { get; set; } = null;

        public int id { get; set; } = 0;
        public string name { get; set; } = null;
        public string adr { get; set; } = null;

        public string supervisor { get; set; } = null;

        public string planStartDate { get; set; } = null;
        public string factStartDate { get; set; } = null;

        public string planFinishDate { get; set; } = null;
        public string factFinishDate { get; set; } = null;

        public int planWorkPrice { get; set; } = 0;
        public int factWorkPrice { get; set; } = 0;

        public int planMaterialPrice { get; set; } = 0;
        public int factMaterialPrice { get; set; } = 0;
    }
}

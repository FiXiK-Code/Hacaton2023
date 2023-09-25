using System;
using System.Collections.Generic;

namespace MVP.Date.Api
{
    public class TaskParam
    {
        public string filterTasks { get; set; } = null;
        public string[] filterSupervisor { get; set; } = null;
        public string token { get; set; } = null;

        public int id { get; set; } = 0;
        public string name { get; set; } = null;
        public int prijId { get; set; } = 0;
        public string status { get; set; } = null;

        public string planStartDate { get; set; } = null;
        public string factStartDate { get; set; } = null;

        public string planFinishDate { get; set; } = null;
        public string factFinishDate { get; set; } = null;

        public string planPayDate { get; set; } = null;
        public string factPayDate { get; set; } = null;

        public int planedPrice { get; set; } = 0;
        public int factPrice { get; set; } = 0;

        public string parentTaskName { get; set; } = null;
        public int parentTaskId { get; set; } = 0;

        public string materials { get; set; } = null;
        public string necesseMaterials { get; set; } = null;

        public string supervisor { get; set; } = null;

        public int planMaterialPrice { get; set; } = 0;
        public int factMaterialPrice { get; set; } = 0;
    }
}

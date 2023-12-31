﻿using System;
using System.Collections.Generic;

namespace MVP.Date.Models
{
	public class Task
	{
		public int id { get; set; }
		public string name { get; set; }
		public int prijId { get; set; }
		public string status { get; set; }
        public string photoPath { get; set; }

        public DateTime planStartDate { get; set; }
        public DateTime factStartDate { get; set; }

        public DateTime planFinishDate { get; set; }
        public DateTime factFinishDate { get; set; }

        public DateTime planPayDate { get; set; }
        public DateTime factPayDate { get; set; }

        public int planedPrice { get; set; }
		public int factPrice { get; set; }

		public string parentTaskName { get; set; }
		public int parentTaskId { get; set; }

        public string materials { get; set; }
		public string necesseMaterials { get; set; }

		public string supervisor { get; set; }

		public int planedMaterialPrice { get; set; }
		public int factMaterialPrice { get; set; }

    }
}


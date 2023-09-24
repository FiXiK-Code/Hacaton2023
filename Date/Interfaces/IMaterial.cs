using MVP.Date.Api;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using System.Xml.Linq;

namespace MVP.Date.Interfaces
{
    public interface IMaterial
    {
        public List<Material> GetAllMaterials { get; }
        public Material GetMaterial(int id);
        public bool RedactToDb(int id,
                string name,
                string category,
                string countName,
                int planCount,
                int factCount,
                int planPrice,
                int factPrice,
                string status,
                string taskName,
                string provider,
                DateTime planPayDate,
                DateTime factPayDate);

        public void AddToDb(Material material);
    }
}

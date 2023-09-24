using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVP.Date.Repository
{
    public class MaterialRep : IMaterial
    {
        private readonly AppDB _appDB;

        public MaterialRep(AppDB appDB)
        {
            _appDB = appDB;
        }

        public List<Material> GetAllMaterials => _appDB.DBMaterial.ToList();

        public void AddToDb(Material material)
        {
            _appDB.DBMaterial.Add(material);
            _appDB.SaveChanges();
        }

        public Material GetMaterial(int id)
        {
            try
            {
                return _appDB.DBMaterial.FirstOrDefault(p => p.id == id);
            }
            catch (Exception) { return null; }
        }

        public bool RedactToDb(int id, string name, string category, string countName, int planCount, int factCount, int planPrice, int factPrice, string status, string taskName, string provider, DateTime planPayDate, DateTime factPayDate)
        {
            throw new NotImplementedException();
        }
    }
}

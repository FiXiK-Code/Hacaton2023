using MVP.Date.Interfaces;
using MVP.Date.Models;
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
    }
}

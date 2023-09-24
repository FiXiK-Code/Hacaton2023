using MVP.Date.Models;
using System.Collections.Generic;
namespace MVP.Date.Interfaces
{
    public interface IMaterial
    {
        public List<Material> GetAllMaterials { get; }
    }
}

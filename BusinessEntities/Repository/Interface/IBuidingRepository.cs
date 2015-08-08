using BusinessEntities.Common;
using BusinessEntities.Repository.Record;
using Model.Model;

namespace BusinessEntities.Repository.Interface
{
    public interface IBuildingRepository : IGetRepository<Building>, ISaveRepository<BuildingRecord>
    {

    }
}

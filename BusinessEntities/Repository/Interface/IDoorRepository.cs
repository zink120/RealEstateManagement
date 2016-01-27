using BusinessEntities.Common;
using BusinessEntities.Repository.Record;
using Model.Model.Dao;

namespace BusinessEntities.Repository.Interface
{
    public interface IDoorRepository : IGetRepository<IDoor>, ISaveRepository<DoorRecord>
    {
    }
}

using BusinessEntities.Common;
using BusinessEntities.Repository.Record;
using Model.Model;

namespace BusinessEntities.Repository.Interface
{
    public interface IDoorRepository : IGetRepository<Door>, ISaveRepository<DoorRecord>
    {
        Building GetBuilding(Door door);
    }
}

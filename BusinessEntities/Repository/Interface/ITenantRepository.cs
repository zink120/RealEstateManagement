using BusinessEntities.Common;
using BusinessEntities.Repository.Record;
using Model.Model.Dao;

namespace BusinessEntities.Repository.Interface
{
    public interface ITenantRepository : IGetRepository<ITenant>, ISaveRepository<TenantRecord>
    {
    }
}

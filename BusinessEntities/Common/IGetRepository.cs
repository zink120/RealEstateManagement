using System.Collections.Generic;

namespace BusinessEntities.Common
{
    public interface IGetRepository<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void ClearCache();
    }
}

using System.Collections.Generic;

namespace Model.DB.Interface
{
    public interface IGenericDao<T>
    {
        void Delete(T record);
        void ClearTable();
        IEnumerable<T> Fetch();
        T Save(T record);
    }
}

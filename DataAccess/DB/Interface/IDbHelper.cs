using System.Collections.Generic;

namespace Model.DB.Interface
{
    public interface IDbHelper
    {
        void Execute(string sql, object parameters = null);
        T ExecuteScalar<T>(string sql, object parameters = null);
        IEnumerable<T> Query<T>(string sql, object parameters = null);
    }
}

using Dapper;
using Model.DB.Interface;
using System.Collections.Generic;

namespace Model.DB
{
    public class DbHelper : IDbHelper
    {
        private readonly IDb _db;

        public DbHelper(IDb db)
        {
            _db = db;
        }

        public T ExecuteScalar<T>(string sql, object parameters)
        {
            using (var dbConnection = _db.DbConnection())
            {
                dbConnection.Open();
                return dbConnection.ExecuteScalar<T>(sql, parameters);
            }
        }

        public void Execute(string sql, object parameters)
        {
            using (var dbConnection = _db.DbConnection())
            {
                dbConnection.Open();
                dbConnection.Execute(sql, parameters);
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            using (var dbConnection = _db.DbConnection())
            {
                dbConnection.Open();
                return dbConnection.Query<T>(sql, parameters);
            }
        }

    }
}

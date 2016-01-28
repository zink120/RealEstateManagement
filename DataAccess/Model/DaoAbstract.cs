using System.Collections.Generic;
using Model.DB.Interface;

namespace Model.Model
{
    public abstract class DaoAbstract<T> : IGenericDao<T>, ICreateDbTable
    {
        protected readonly string TableName;
        protected readonly IDbHelper Db;

        protected DaoAbstract(IDbHelper db, string tableName)
        {
            Db = db;
            TableName = tableName;
        }

        public virtual void DropTable()
        {
            var createQuery = $@"DROP TABLE IF EXISTS {TableName}";
            Db.Execute(createQuery);
        }

        public abstract void CreateTable();
        public abstract void Delete(T record);

        public virtual void ClearTable()
        {
            var deleteQuery = $"DELETE {TableName}";
            Db.Execute(deleteQuery);
        }

        public virtual IEnumerable<T> Fetch()
        {
            return Db.Query<T>($"SELECT * FROM {TableName}");
        }

        public abstract T Save(T record);
    }
}
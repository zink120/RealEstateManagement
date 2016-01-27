using Model.DB.Interface;
using System;
using System.Collections.Generic;

namespace Model.Model.Dao
{
    public class TenantRecord
    {
        public int TenantID { get; internal set; }
        public int DoorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }

    public interface ITenantDao : IGenericDao<TenantRecord>, ICreateDbTable {}

    public class TenantDao : ITenantDao
    {
        private IDbHelper _db;
        private const string _tableName = "Tenant";
        public TenantDao(IDbHelper db)
        {
            _db = db;
        }

        public void CreateTable()
        {
            var createQuery = string.Format(@"DROP TABLE IF EXISTS {0};
                                            CREATE TABLE Tenant (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {5} INTEGER NOT NULL,
                                            {2} VARCHAR(50) NOT NULL,
                                            {3} VARCHAR(50) NOT NULL,
                                            {4} DATETIME NOT NULL);",
                                            _tableName,
                                            nameof(TenantRecord.TenantID),
                                            nameof(TenantRecord.FirstName),
                                            nameof(TenantRecord.LastName),
                                            nameof(TenantRecord.LastModifiedDate),
                                            nameof(TenantRecord.DoorID));
            _db.Execute(createQuery);
        }

        public void Delete(TenantRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}",
                                            _tableName,
                                            nameof(record.TenantID));
            _db.Execute(deleteQuery, record);
        }

        public void ClearTable()
        {
            var deleteQuery = string.Format("DELETE {0}", _tableName);
            _db.Execute(deleteQuery);
        }

        public IEnumerable<TenantRecord> Fetch()
        {
            return _db.Query<TenantRecord>($"SELECT * FROM {_tableName}");
        }

        public TenantRecord Save(TenantRecord record)
        {
            if (record.TenantID <= 0)
                record.TenantID = Insert(record);
            else
                Update(record);
            return record;
        }

        private int Insert(TenantRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var insertQuery = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}) VALUES (@{1}, @{2}, @{3}, @{4}); SELECT last_insert_rowid();",
                                            _tableName,
                                            nameof(record.FirstName),
                                            nameof(record.LastName),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.DoorID));


            return _db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(TenantRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2}, {3}=@{3}, {5}=@{5} WHERE {4}=@{4}",
                                            _tableName,
                                            nameof(record.FirstName),
                                            nameof(record.LastName),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.TenantID),
                                            nameof(record.DoorID));

            _db.Execute(updateQuery, record);
        }


    }
}

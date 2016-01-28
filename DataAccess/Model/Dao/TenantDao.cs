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
        public DateTime LastModifiedDate { get; internal set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }

    public interface ITenantDao : IGenericDao<TenantRecord>, ICreateDbTable {}

    public class TenantDao : DaoAbstract<TenantRecord>, ITenantDao
    {
        public TenantDao(IDbHelper db) : base (db, "Tenant")
        {
        }

        public override void CreateTable()
        {
            var createQuery = string.Format(@"DROP TABLE IF EXISTS {0};
                                            CREATE TABLE Tenant (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {5} INTEGER NOT NULL,
                                            {2} VARCHAR(50) NOT NULL,
                                            {3} VARCHAR(50) NOT NULL,
                                            {4} DATETIME NOT NULL,
                                            FOREIGN KEY ({5}) REFERENCES Door({5}));",
                                            TableName,
                                            nameof(TenantRecord.TenantID),
                                            nameof(TenantRecord.FirstName),
                                            nameof(TenantRecord.LastName),
                                            nameof(TenantRecord.LastModifiedDate),
                                            nameof(TenantRecord.DoorID));
            Db.Execute(createQuery);
        }

        public override void Delete(TenantRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}",
                                            TableName,
                                            nameof(record.TenantID));
            Db.Execute(deleteQuery, record);
        }

        public override TenantRecord Save(TenantRecord record)
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
                                            TableName,
                                            nameof(record.FirstName),
                                            nameof(record.LastName),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.DoorID));


            return Db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(TenantRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2}, {3}=@{3}, {5}=@{5} WHERE {4}=@{4}",
                                            TableName,
                                            nameof(record.FirstName),
                                            nameof(record.LastName),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.TenantID),
                                            nameof(record.DoorID));

            Db.Execute(updateQuery, record);
        }


    }
}

using System;
using Model.DB.Interface;

namespace Model.Model.Dao
{
    public class TenantInteractionRecord
    {
        public int TenantInteractionID { get; internal set; }
        public int TenantID { get; set; }
        public DateTime AsOfDate { get; set; }
        public string Comment { get; set; }
        public DateTime LastModifiedDate { get; internal set; }
    }

    public interface ITenantInteractionDao : IGenericDao<TenantInteractionRecord>, ICreateDbTable { }

    public class TenantInteractionDao : DaoAbstract<TenantInteractionRecord>, ITenantInteractionDao
    {
        public TenantInteractionDao(IDbHelper db):base(db, "TenantInteraction")
        {
        }

        public override void CreateTable()
        {
            var createQuery = $@"CREATE TABLE {TableName} (                                           
                                 {nameof(TenantInteractionRecord.TenantInteractionID)} INTEGER PRIMARY KEY AUTOINCREMENT,
                                 {nameof(TenantInteractionRecord.TenantID)} INTEGER NOT NULL,
                                 {nameof(TenantInteractionRecord.AsOfDate)} DATE NOT NULL,
                                 {nameof(TenantInteractionRecord.Comment)} TEXT NOT NULL,
                                 {nameof(TenantInteractionRecord.LastModifiedDate)} DATETIME NOT NULL,
                                FOREIGN KEY ({nameof(TenantInteractionRecord.TenantID)}) 
                                REFERENCES Tenant({nameof(TenantInteractionRecord.TenantID)}));";
            Db.Execute(createQuery);
        }

        public override void Delete(TenantInteractionRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}",
                                            TableName,
                                            nameof(record.TenantInteractionID));
            Db.Execute(deleteQuery, record);
        }
        
        public override TenantInteractionRecord Save(TenantInteractionRecord record)
        {
            if (record.TenantInteractionID <= 0)
                record.TenantInteractionID = Insert(record);
            else
                Update(record);
            return record;
        }

        private int Insert(TenantInteractionRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var insertQuery = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}) VALUES (@{1}, @{2}, @{3}, @{4}); SELECT last_insert_rowid();",
                TableName,
                nameof(record.TenantID),
                nameof(record.AsOfDate),
                nameof(record.Comment),
                nameof(record.LastModifiedDate));


            return Db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(TenantInteractionRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2}, {3}=@{3}, {5}=@{5} WHERE {4}=@{4}",
                                            TableName,
                                            nameof(record.TenantID),
                                            nameof(record.AsOfDate),
                                            nameof(record.Comment),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.TenantInteractionID));

            Db.Execute(updateQuery, record);
        }
    }
}

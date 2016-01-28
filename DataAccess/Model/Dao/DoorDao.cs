using Model.DB.Interface;
using System;
using System.Collections.Generic;

namespace Model.Model.Dao
{
    public class DoorRecord
    {
        public int DoorID { get; internal set; }
        public int BuildingID { get; set; }
        public string Address { get; set; }
        public DateTime LastModifiedDate { get; internal set; }
    }

    public interface IDoorDao : IGenericDao<DoorRecord>, ICreateDbTable { }
    
    public class DoorDao : DaoAbstract<DoorRecord>, IDoorDao
    {
        public DoorDao(IDbHelper db) : base(db, "Door")
        {
        }
        
        public override void CreateTable()
        {
            var createQuery = string.Format(@"DROP TABLE IF EXISTS {0};
                                            CREATE TABLE {0} (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {2} INTEGER NOT NULL,
                                            {3} VARCHAR(250) NOT NULL,
                                            {4} DATETIME NOT NULL,
                                            FOREIGN KEY ({2}) REFERENCES Building({2})
                                            );",
                                            TableName,
                                            nameof(DoorRecord.DoorID),
                                            nameof(DoorRecord.BuildingID),
                                            nameof(DoorRecord.Address),
                                            nameof(DoorRecord.LastModifiedDate));
            Db.Execute(createQuery);
        }

        public override void Delete(DoorRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}", 
                                            TableName,
                                            nameof(DoorRecord.DoorID));
            Db.Execute(deleteQuery, record);
        }

        public override DoorRecord Save(DoorRecord record)
        {
            if (record.DoorID <= 0)
                record.DoorID = Insert(record);
            else
                Update(record);
            return record;
        }

        private int Insert(DoorRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var insertQuery = string.Format(@"INSERT INTO {0}({1}, {2}, {3}) VALUES (@{1}, @{2}, @{3}); 
                                              SELECT last_insert_rowid();",
                                            TableName,
                                            nameof(DoorRecord.BuildingID),
                                            nameof(DoorRecord.Address),
                                            nameof(DoorRecord.LastModifiedDate));


            return Db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(DoorRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2}, {3}=@{3} WHERE {4}=@{4}",
                                            TableName,
                                            nameof(DoorRecord.BuildingID),
                                            nameof(DoorRecord.Address),
                                            nameof(DoorRecord.LastModifiedDate),
                                            nameof(DoorRecord.DoorID));

            Db.Execute(updateQuery, record);
        }
    }
}

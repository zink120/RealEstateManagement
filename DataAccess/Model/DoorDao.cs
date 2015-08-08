using Model.DB.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class DoorRecord
    {
        public int DoorID { get; set; }
        public int BuildingID { get; set; }
        public string Address { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public interface IDoorDao : IGenericDao<DoorRecord>, ICreateDbTable { }
    
    public class DoorDao : IDoorDao
    {
        private IDbHelper _db;
        private const string _tableName = "Door";
        public DoorDao(IDbHelper db)
        {
            _db = db;
        }

        public void CreateTable()
        {
            var createQuery = string.Format(@"DROP TABLE IF EXISTS {0};
                                            CREATE TABLE {0} (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {2} INTEGER NOT NULL,
                                            {3} VARCHAR(250) NOT NULL,
                                            {4} DATETIME NOT NULL,
                                            FOREIGN KEY ({2}) REFERENCES Building({5})
                                            );",
                                            _tableName,
                                            nameof(DoorRecord.DoorID),
                                            nameof(DoorRecord.BuildingID),
                                            nameof(DoorRecord.Address),
                                            nameof(DoorRecord.LastModifiedDate),
                                            nameof(BuildingRecord.BuildingID));
            _db.Execute(createQuery);
        }

        public void Delete(DoorRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}", 
                                            _tableName,
                                            nameof(DoorRecord.DoorID));
            _db.Execute(deleteQuery, record);
        }

        public IEnumerable<DoorRecord> Fetch()
        {
            return _db.Query<DoorRecord>(string.Format("SELECT * FROM {0}", _tableName));
        }

        public DoorRecord Save(DoorRecord record)
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
                                            _tableName,
                                            nameof(DoorRecord.BuildingID),
                                            nameof(DoorRecord.Address),
                                            nameof(DoorRecord.LastModifiedDate));


            return _db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(DoorRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2}, {3}=@{3} WHERE {4}=@{4}",
                                            _tableName,
                                            nameof(DoorRecord.BuildingID),
                                            nameof(DoorRecord.Address),
                                            nameof(DoorRecord.LastModifiedDate),
                                            nameof(DoorRecord.DoorID));

            _db.Execute(updateQuery, record);
        }
    }
}

using Model.DB.Interface;
using System;
using System.Collections.Generic;

namespace Model.Model
{
    public class BuildingRecord
    {
        public int BuildingID { get; internal set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public interface IBuildingDao : IGenericDao<BuildingRecord>, ICreateDbTable {}

    public class BuildingDao : IBuildingDao
    {
        private IDbHelper _db;
        private const string _tabName = "Building";
        public BuildingDao(IDbHelper db)
        {
            _db = db;
        }

        public void CreateTable()
        {
            var createQuery = string.Format(@"DROP TABLE IF EXISTS {0};
                                            CREATE TABLE Building (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {2} VARCHAR(250) NOT NULL,
                                            {3} DATETIME NOT NULL);",
                                            _tabName,
                                            nameof(BuildingRecord.BuildingID),
                                            nameof(BuildingRecord.Name),
                                            nameof(BuildingRecord.LastModifiedDate));
            _db.Execute(createQuery);
        }

        public void Delete(BuildingRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}",
                                            _tabName,
                                            nameof(record.BuildingID));
            _db.Execute(deleteQuery, record);
        }

        public IEnumerable<BuildingRecord> Fetch()
        {
            return _db.Query<BuildingRecord>($"SELECT * FROM {_tabName}");
        }

        public BuildingRecord Save(BuildingRecord record)
        {
            if (record.BuildingID <= 0)
                record.BuildingID = Insert(record);
            else
                Update(record);
            return record;
        }

        private int Insert(BuildingRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var insertQuery = string.Format("INSERT INTO {0}({1}, {2}) VALUES (@{1}, @{2}); SELECT last_insert_rowid();",
                                            _tabName,
                                            nameof(record.Name),
                                            nameof(record.LastModifiedDate));


            return _db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(BuildingRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2} WHERE {3}=@{3}",
                                            _tabName,
                                            nameof(record.Name),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.BuildingID));

            _db.Execute(updateQuery, record);
        }


    }
}

using Model.DB.Interface;
using System;
using System.Collections.Generic;

namespace Model.Model.Dao
{
    public class BuildingRecord
    {
        public int BuildingID { get; internal set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; internal set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public interface IBuildingDao : IGenericDao<BuildingRecord>, ICreateDbTable {}

    public class BuildingDao : DaoAbstract<BuildingRecord>, IBuildingDao
    {
        public BuildingDao(IDbHelper db) : base (db, "Building")
        {
        }


        public override void CreateTable()
        {
            var createQuery = string.Format(@"DROP TABLE IF EXISTS {0};
                                            CREATE TABLE  {0} (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {2} VARCHAR(250) NOT NULL,
                                            {3} DATETIME NOT NULL);",
                                            TableName,
                                            nameof(BuildingRecord.BuildingID),
                                            nameof(BuildingRecord.Name),
                                            nameof(BuildingRecord.LastModifiedDate));
            Db.Execute(createQuery);
        }

        public override void Delete(BuildingRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}",
                                            TableName,
                                            nameof(record.BuildingID));
            Db.Execute(deleteQuery, record);
        }

        public override BuildingRecord Save(BuildingRecord record)
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
                                            TableName,
                                            nameof(record.Name),
                                            nameof(record.LastModifiedDate));


            return Db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(BuildingRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {2}=@{2} WHERE {3}=@{3}",
                                            TableName,
                                            nameof(record.Name),
                                            nameof(record.LastModifiedDate),
                                            nameof(record.BuildingID));

            Db.Execute(updateQuery, record);
        }


    }
}

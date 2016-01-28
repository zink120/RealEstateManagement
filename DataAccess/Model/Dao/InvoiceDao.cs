using System;
using Model.DB.Interface;

namespace Model.Model.Dao
{
    public class InvoiceRecord
    {
        public int InvoiceID { get; internal set; }
        public DateTime AsOfDate { get; set; }
        public decimal Amount { get; set; }
        public int DoorID { get; set; }
        public DateTime LastModifiedDate { get; internal set; }
    }

    public interface IInvoiceDao : IGenericDao<InvoiceRecord>, ICreateDbTable
    {
    }

    public class InvoiceDao : DaoAbstract<InvoiceRecord>, IInvoiceDao
    {
        public InvoiceDao(IDbHelper db) : base(db, "Invoice")
        {
        }

        public override void CreateTable()
        {
            var createQuery = string.Format(@"CREATE TABLE {0} (
                                            {1} INTEGER PRIMARY KEY AUTOINCREMENT,
                                            {3} DATE NOT NULL,
                                            {4} DECIMAL(10,2) NOT NULL,
                                            {5} INT NOT NULL,
                                            {2} DATETIME NOT NULL,
                                            FOREIGN KEY ({5}) REFERENCES Door({5}));",
                TableName,
                nameof(InvoiceRecord.InvoiceID),
                nameof(InvoiceRecord.LastModifiedDate),
                nameof(InvoiceRecord.AsOfDate),
                nameof(InvoiceRecord.Amount),
                nameof(InvoiceRecord.DoorID));
            Db.Execute(createQuery);
        }

        public override void Delete(InvoiceRecord record)
        {
            var deleteQuery = string.Format("DELETE {0} WHERE {1}=@{1}",
                TableName,
                nameof(InvoiceRecord.InvoiceID));
            Db.Execute(deleteQuery, record);
        }

        public override InvoiceRecord Save(InvoiceRecord record)
        {
            if (record.InvoiceID <= 0)
                record.InvoiceID = Insert(record);
            else
                Update(record);
            return record;
        }

        private int Insert(InvoiceRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var insertQuery = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4}) VALUES (@{1}, @{2}, @{3}, @{4}); 
                                              SELECT last_insert_rowid();",
                TableName,
                nameof(InvoiceRecord.LastModifiedDate),
                nameof(InvoiceRecord.AsOfDate),
                nameof(InvoiceRecord.Amount),
                nameof(InvoiceRecord.DoorID));

            return Db.ExecuteScalar<int>(insertQuery, record);
        }

        private void Update(InvoiceRecord record)
        {
            record.LastModifiedDate = DateTime.Now;
            var updateQuery = string.Format("UPDATE {0} SET {1}=@{1}, {3}=@{3}, {4}=@{4}, {5}=@{5} WHERE {2}=@{2}",
                TableName,
                nameof(InvoiceRecord.LastModifiedDate),
                nameof(InvoiceRecord.InvoiceID),
                nameof(InvoiceRecord.AsOfDate),
                nameof(InvoiceRecord.Amount),
                nameof(InvoiceRecord.DoorID));

            Db.Execute(updateQuery, record);
        }
    }
}
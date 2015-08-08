using System.Data.Common;

namespace Model.DB.Interface
{
    public interface IDb
    {
        string DbPath { get; }
        DbConnection DbConnection();
    }
}

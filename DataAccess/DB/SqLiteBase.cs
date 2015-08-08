using Model.DB.Interface;
using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace Model.DB
{
    public class SqLiteBase : IDb
    {
        public string DbPath
        {
            get 
            {
                return "C:\\Users\\Alexandre\\Source\\Repos\\RealEstateManagement\\DataAccess\\DB\\RealEstateManagementDB.db";
            }
        }

        public DbConnection DbConnection()
        {
            return new SQLiteConnection("Data Source = " + DbPath + ";Version=3;Password=RealEstateManagementDBPassword;foreign keys=ON;");
        }
    }
}

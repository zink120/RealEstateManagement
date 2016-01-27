using Model.DB;
using Model.DB.Interface;
using Model.Model.Dao;
using System;

namespace Model.Model
{
    public class ModelFactory : IModelFactory
    {
        #region Variables

        private Lazy<IBuildingDao> _building;
        private Lazy<IDoorDao> _door;
        private Lazy<ITenantDao> _tenant;
        private IDbHelper _dbHelper;

        #endregion

        #region Properties

        public IBuildingDao Building { get { return _building.Value; } }
        public IDoorDao Door { get { return _door.Value; } }
        public ITenantDao Tenant { get { return _tenant.Value; } }
            
        #endregion

        public ModelFactory(IDb db)
        {
            _dbHelper = new DbHelper(db);

            InitLazeDao();
        }

        private void InitLazeDao()
        {
            _building = new Lazy<IBuildingDao>(() => new BuildingDao(_dbHelper), true);
            _door = new Lazy<IDoorDao>(() => new DoorDao(_dbHelper), true);
            _tenant = new Lazy<ITenantDao>(() => new TenantDao(_dbHelper), true);
        }

        public void CreateDataTable()
        {
            _tenant.Value.CreateTable();
            _door.Value.CreateTable();
            _building.Value.CreateTable();
        }

    }
}

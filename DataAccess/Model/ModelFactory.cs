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
        private Lazy<ITenantInteractionDao> _tenantInteraction;
        private Lazy<IInvoiceDao> _invoice;
        private readonly IDbHelper _dbHelper;

        #endregion

        #region Properties

        public IBuildingDao Building => _building.Value;
        public IDoorDao Door => _door.Value;
        public ITenantDao Tenant => _tenant.Value;
        public ITenantInteractionDao TenantInteraction => _tenantInteraction.Value;
        public IInvoiceDao Invoice => _invoice.Value;

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
            _tenantInteraction = new Lazy<ITenantInteractionDao>(()=>new TenantInteractionDao(_dbHelper), true);
            _invoice = new Lazy<IInvoiceDao>(()=>new InvoiceDao(_dbHelper), true);
        }

        public void DropDataTable()
        {
            _invoice.Value.DropTable();
            _tenantInteraction.Value.DropTable();
            _tenant.Value.DropTable();
            _door.Value.DropTable();
            _building.Value.DropTable();
        }

        public void CreateDataTable()
        {
            _building.Value.CreateTable();
            _door.Value.CreateTable();
            _tenant.Value.CreateTable();
            _invoice.Value.CreateTable();
            _tenantInteraction.Value.CreateTable();

        }

    }
}

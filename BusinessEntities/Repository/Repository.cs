using System;
using Model.Model;
using BusinessEntities.Repository.Interface;

namespace BusinessEntities.Repository
{
    public class Repository : IRepository
    {
        public Repository(IModelFactory factory)
        {
            _tenant = new Lazy<ITenantRepository>(() => new TenantRepository(factory.Tenant), true);
            _door = new Lazy<IDoorRepository>(() => new DoorRepository(_tenant, factory.Door), true);
            _building = new Lazy<IBuildingRepository>(()=>new BuildingRepository(_door, factory.Building), true);
            
        }

        private Lazy<IBuildingRepository> _building;
        private Lazy<IDoorRepository> _door;
        private Lazy<ITenantRepository> _tenant;

        public IBuildingRepository Building => _building.Value;
        public IDoorRepository Door => _door.Value;
        public ITenantRepository Tenant => _tenant.Value;
    }
}

using System;
using Model.Model;
using BusinessEntities.Repository.Interface;
using BusinessEntities.Repository.Record;

namespace BusinessEntities.Repository
{
    public class Repository : IRepository
    {
        public Repository(IModelFactory factory)
        {
            _tenantInteraction = new Lazy<ITenantInteractionRepository>(() => new TenantInteractionRepository(factory.TenantInteraction), true);
            _tenant = new Lazy<ITenantRepository>(() => new TenantRepository(_tenantInteraction, factory.Tenant), true);
            _door = new Lazy<IDoorRepository>(() => new DoorRepository(_tenant, factory.Door), true);
            _building = new Lazy<IBuildingRepository>(()=>new BuildingRepository(_door, factory.Building), true); 
        }

        private readonly Lazy<IBuildingRepository> _building;
        private readonly Lazy<IDoorRepository> _door;
        private readonly Lazy<ITenantRepository> _tenant;
        private readonly Lazy<ITenantInteractionRepository> _tenantInteraction; 

        public IBuildingRepository Building => _building.Value;
        public IDoorRepository Door => _door.Value;
        public ITenantRepository Tenant => _tenant.Value;
        public ITenantInteractionRepository TenantInteraction => _tenantInteraction.Value;
    }
}

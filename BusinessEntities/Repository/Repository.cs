using System;
using Model.DB.Interface;
using Model.Model;
using BusinessEntities.Repository.Interface;

namespace BusinessEntities.Repository
{
    public class Repository : IRepository
    {
        public Repository(IDb db)
        {
            IModelFactory factory = new ModelFactory(db);

            _building = new Lazy<IBuildingRepository>(()=>new BuildingRepository(factory.Building), true);
            _door = new Lazy<IDoorRepository>(() => new DoorRepository(this, factory.Door), true);
        }

        private Lazy<IBuildingRepository> _building;
        private Lazy<IDoorRepository> _door;

        public IBuildingRepository Building { get { return _building.Value; } }
        public IDoorRepository Door { get { return _door.Value; } }
    }
}

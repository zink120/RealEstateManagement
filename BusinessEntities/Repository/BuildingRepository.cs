using AutoMapper;
using Model.Model.Dao;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using BusinessEntities.Exceptions;
using BusinessEntities.Repository.Interface;
using BusinessEntities.Repository.Record;
using System;

namespace BusinessEntities.Repository
{
    public class BuildingRepository : IBuildingRepository
    {
        private ConcurrentDictionary<int, IBuilding> _cache;
        private readonly object _lock = new object();
        private readonly IBuildingDao _dao;
        private readonly Lazy<IDoorRepository> _doorRepository;

        public BuildingRepository(Lazy<IDoorRepository> doorRepository, IBuildingDao dao)
        {
            _doorRepository = doorRepository;
            _dao = dao;
            lock(Constant.AutoMapperLock)
                Mapper.CreateMap<BuildingRecord, Building>();
        }

        public IBuilding Get(int id)
        {
            IBuilding data;
            if (_cache != null && _cache.TryGetValue(id, out data))
                return data;
            lock (_lock)
            {
                LoadCache();
                if (_cache.TryGetValue(id, out data))
                    return data;
                throw new IdNotFoundException(id, nameof(BuildingRepository), nameof(BuildingRepository.Get));
            }
        }

        public IEnumerable<IBuilding> GetAll()
        {
            LoadCache();
            return _cache.Values.ToList();
        }

        public void ClearCache()
        {
            lock(_lock)
                _cache = null;
        }

        public void Save(BuildingRecord building)
        {
            var record = _dao.Save(building);
            if (_cache == null) return;
            lock(_lock)
            {
                if (_cache == null) return;
                LoadData(record, _cache);
            }
        }

        private void LoadCache()
        {
            if (_cache != null) return;
            lock(_lock)
            {
                if (_cache != null) return;
                var cache = new ConcurrentDictionary<int, IBuilding>();
                foreach (var data in _dao.Fetch())
                    LoadData(data, cache);
                _cache = cache;
            }
        }

        private void LoadData(BuildingRecord record, ConcurrentDictionary<int, IBuilding> cache)
        {
            int buildingID = record.BuildingID;
            Lazy<IEnumerable<IDoor>> doors = new Lazy<IEnumerable<IDoor>>(() => _doorRepository.Value.GetAll().Where(_=>_.BuildingID == buildingID));

            cache[record.BuildingID] = Mapper.Map(record, new Building(doors));
        }
    }
}

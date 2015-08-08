using AutoMapper;
using Model.Model;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using BusinessEntities.Exceptions;
using BusinessEntities.Repository.Interface;
using BusinessEntities.Repository.Record;

namespace BusinessEntities.Repository
{
    public class BuildingRepository : IBuildingRepository
    {
        private ConcurrentDictionary<int, Building> _cache;
        private readonly object _lock = new object();
        private readonly IBuildingDao _dao;

        public BuildingRepository(IBuildingDao dao)
        {
            _dao = dao;
            lock(Constant.AutoMapperLock)
                Mapper.CreateMap<BuildingRecord, Building>();
        }

        public Building Get(int id)
        {
            Building data;
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

        public IEnumerable<Building> GetAll()
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
                ConcurrentDictionary<int, Building> cache = new ConcurrentDictionary<int, Building>();
                foreach (var data in _dao.Fetch())
                    LoadData(data, cache);
                _cache = cache;
            }
        }

        private void LoadData(BuildingRecord record, ConcurrentDictionary<int, Building> cache)
        {
            cache[record.BuildingID] = Mapper.Map(record, new Building(this));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Model.Model;
using System.Collections.Concurrent;
using BusinessEntities.Repository.Record;
using AutoMapper;
using BusinessEntities.Exceptions;
using BusinessEntities.Repository.Interface;
using System;

namespace BusinessEntities.Repository
{
    public class DoorRepository : IDoorRepository
    {
        private ConcurrentDictionary<int, Door> _cache;
        private readonly object _lock = new object();
        private readonly IRepository _repository;
        private IDoorDao _dao;

        public DoorRepository(IRepository repository, IDoorDao dao)
        {
            _repository = repository;
            _dao = dao;
            lock (Constant.AutoMapperLock)
                Mapper.CreateMap<DoorRecord, Door>();
        }

        public Door Get(int id)
        {
            Door data;
            if (_cache != null && _cache.TryGetValue(id, out data))
                return data;
            lock (_lock)
            {
                LoadCache();
                if (_cache.TryGetValue(id, out data))
                    return data;
                throw new IdNotFoundException(id, nameof(DoorRepository), nameof(DoorRepository.Get));
            }
        }

        public IEnumerable<Door> GetAll()
        {
            LoadCache();
            return _cache.Values.ToList();
        }

        public void ClearCache()
        {
            lock (_lock)
                _cache = null;
        }

        public void Save(DoorRecord Door)
        {
            var record = _dao.Save(Door);
            if (_cache == null) return;
            lock (_lock)
            {
                if (_cache == null) return;
                LoadData(record, _cache);
            }
        }

        private void LoadCache()
        {
            if (_cache != null) return;
            lock (_lock)
            {
                if (_cache != null) return;
                ConcurrentDictionary<int, Door> cache = new ConcurrentDictionary<int, Door>();
                foreach (var data in _dao.Fetch())
                    LoadData(data, cache);
                _cache = cache;
            }
        }

        private void LoadData(DoorRecord record, ConcurrentDictionary<int, Door> cache)
        {
            cache[record.DoorID] = Mapper.Map(record, new Door(this));
        }

        public Building GetBuilding(Door door)
        {
            return _repository.Building.Get(door.BuildingID);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Model.Model.Dao;
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
        private ConcurrentDictionary<int, IDoor> _cache;
        private readonly object _lock = new object();
        private IDoorDao _dao;
        private Lazy<ITenantRepository> _tenantRepository;

        public DoorRepository(Lazy<ITenantRepository> tenantRepository, IDoorDao dao)
        {
            _tenantRepository = tenantRepository;
            _dao = dao;
            lock (Constant.AutoMapperLock)
                Mapper.CreateMap<DoorRecord, Door>();
        }

        public IDoor Get(int id)
        {
            IDoor data;
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

        public IEnumerable<IDoor> GetAll()
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
                var cache = new ConcurrentDictionary<int, IDoor>();
                foreach (var data in _dao.Fetch())
                    LoadData(data, cache);
                _cache = cache;
            }
        }

        private void LoadData(DoorRecord record, ConcurrentDictionary<int, IDoor> cache)
        {
            int doorID = record.DoorID;

            Lazy<IEnumerable<ITenant>> tenants = new Lazy<IEnumerable<ITenant>>(()=>_tenantRepository.Value.GetAll().Where(_=>_.DoorID == doorID));
            cache[record.DoorID] = Mapper.Map(record, new Door(tenants));
        }
    }
}

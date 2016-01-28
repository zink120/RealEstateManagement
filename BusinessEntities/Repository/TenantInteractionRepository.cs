using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessEntities.Exceptions;
using BusinessEntities.Repository.Interface;
using Model.Model.Dao;

namespace BusinessEntities.Repository.Record
{
    public class TenantInteractionRepository : ITenantInteractionRepository
    {
        private ConcurrentDictionary<int, ITenantInteraction> _cache;
        private readonly ITenantInteractionDao _dao;
        private readonly object _lock = new object();

        public TenantInteractionRepository(ITenantInteractionDao dao)
        {
            _dao = dao;

            lock (Constant.AutoMapperLock)
                Mapper.CreateMap<TenantInteractionRecord, TenantInteraction>();
        }



        public ITenantInteraction Get(int id)
        {
            ITenantInteraction data;
            if (_cache != null && _cache.TryGetValue(id, out data))
                return data;
            lock (_lock)
            {
                LoadCache();
                if (_cache.TryGetValue(id, out data))
                    return data;
                throw new IdNotFoundException(id, nameof(TenantInteractionRepository), nameof(TenantInteractionRepository.Get));
            }
        }

        public IEnumerable<ITenantInteraction> GetAll()
        {
            LoadCache();
            return _cache.Values.ToList();
        }

        public void ClearCache()
        {
            lock (_lock)
                _cache = null;
        }

        public void Save(TenantInteractionRecord Tenant)
        {
            var record = _dao.Save(Tenant);
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
                var cache = new ConcurrentDictionary<int, ITenantInteraction>();
                foreach (var data in _dao.Fetch())
                    LoadData(data, cache);
                _cache = cache;
            }
        }

        private void LoadData(TenantInteractionRecord record, ConcurrentDictionary<int, ITenantInteraction> cache)
        {
            cache[record.TenantID] = Mapper.Map(record, new TenantInteraction());
        }
    }
}
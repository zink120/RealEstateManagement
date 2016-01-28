using System;
using AutoMapper;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using BusinessEntities.Exceptions;
using BusinessEntities.Repository.Interface;
using BusinessEntities.Repository.Record;
using Model.Model.Dao;

namespace BusinessEntities.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private ConcurrentDictionary<int, ITenant> _cache;
        private readonly object _lock = new object();
        private readonly Lazy<ITenantInteractionRepository> _tenantInteraction;
        private readonly ITenantDao _dao;

        public TenantRepository(Lazy<ITenantInteractionRepository> tenantInteraction, ITenantDao dao)
        {
            _tenantInteraction = tenantInteraction;
            _dao = dao;
            lock(Constant.AutoMapperLock)
                Mapper.CreateMap<TenantRecord, Tenant>();
        }

        public ITenant Get(int id)
        {
            ITenant data;
            if (_cache != null && _cache.TryGetValue(id, out data))
                return data;
            lock (_lock)
            {
                LoadCache();
                if (_cache.TryGetValue(id, out data))
                    return data;
                throw new IdNotFoundException(id, nameof(TenantRepository), nameof(TenantRepository.Get));
            }
        }

        public IEnumerable<ITenant> GetAll()
        {
            LoadCache();
            return _cache.Values.ToList();
        }

        public void ClearCache()
        {
            lock(_lock)
                _cache = null;
        }

        public void Save(TenantRecord Tenant)
        {
            var record = _dao.Save(Tenant);
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
                var cache = new ConcurrentDictionary<int, ITenant>();
                foreach (var data in _dao.Fetch())
                    LoadData(data, cache);
                _cache = cache;
            }
        }

        private void LoadData(TenantRecord record, ConcurrentDictionary<int, ITenant> cache)
        {
            var tenantID = record.TenantID;
            var tenantInteraction = new Lazy<IEnumerable<ITenantInteraction>>(() => _tenantInteraction.Value.GetAll().Where(_ => _.TenantID == tenantID));

            cache[record.TenantID] = Mapper.Map(record, new Tenant(tenantInteraction));
        }
    }
}

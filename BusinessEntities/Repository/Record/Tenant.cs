using System;
using Model.Model.Dao;
using System.Collections.Generic;

namespace BusinessEntities.Repository.Record
{
    public class Tenant : TenantRecord, ITenant
    {
        private readonly Lazy<IEnumerable<ITenantInteraction>> _tenantInteraction;

        public Tenant(Lazy<IEnumerable<ITenantInteraction>> tenantInteraction)
        {
            _tenantInteraction = tenantInteraction;
        }

        public IEnumerable<ITenantInteraction> TenantInteractions => _tenantInteraction.Value;
    }
}

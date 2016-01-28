using Model.Model.Dao;
using System;
using System.Collections.Generic;

namespace BusinessEntities.Repository.Record
{
    public class Door : DoorRecord, IDoor
    {
        private readonly Lazy<IEnumerable<ITenant>> _tenants;

        public Door(Lazy<IEnumerable<ITenant>> tenants)
        {
            _tenants = tenants;
        }

        public IEnumerable<ITenant> Tenants => _tenants.Value;
    }
}

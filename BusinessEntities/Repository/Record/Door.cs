using Model.Model.Dao;
using System;
using System.Collections.Generic;

namespace BusinessEntities.Repository.Record
{
    public interface IDoor
    {
        int DoorID { get; }
        int BuildingID { get; }
        string Address { get; }
        DateTime LastModifiedDate { get; }

        IEnumerable<ITenant> Tenants { get; }
    }
    public class Door : DoorRecord, IDoor
    {
        private Lazy<IEnumerable<ITenant>> _tenants;

        public Door(Lazy<IEnumerable<ITenant>> tenants)
        {
            _tenants = tenants;
        }

        public IEnumerable<ITenant> Tenants => _tenants.Value;
    }
}

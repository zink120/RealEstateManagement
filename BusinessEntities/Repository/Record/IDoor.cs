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
}
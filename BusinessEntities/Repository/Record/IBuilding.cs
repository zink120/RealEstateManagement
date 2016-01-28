using System;
using System.Collections.Generic;

namespace BusinessEntities.Repository.Record
{
    public interface IBuilding
    {
        int BuildingID { get; }
        string Name { get; }
        DateTime LastModifiedDate { get; }

        IEnumerable<IDoor> Doors { get; }
    }
}
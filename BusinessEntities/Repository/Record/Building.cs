using Model.Model.Dao;
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
    public class Building : BuildingRecord, IBuilding
    {
        private Lazy<IEnumerable<IDoor>> _doors;

        public Building(Lazy<IEnumerable<IDoor>> doors)
        {
            _doors = doors;
        }

        public IEnumerable<IDoor> Doors => _doors.Value;
        
    }
}

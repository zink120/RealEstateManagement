using Model.Model.Dao;
using System;
using System.Collections.Generic;

namespace BusinessEntities.Repository.Record
{
    public class Building : BuildingRecord, IBuilding
    {
        private readonly Lazy<IEnumerable<IDoor>> _doors;

        public Building(Lazy<IEnumerable<IDoor>> doors)
        {
            _doors = doors;
        }

        public IEnumerable<IDoor> Doors => _doors.Value;
        
    }
}

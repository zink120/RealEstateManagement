using Model.Model;

namespace BusinessEntities.Repository.Record
{
    public class Building : BuildingRecord
    {
        private BuildingRepository _buildingRepository;

        public Building(BuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }
    }
}

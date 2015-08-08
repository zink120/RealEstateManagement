using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Repository.Interface;

namespace BusinessEntities.Repository.Record
{
    public class Door : DoorRecord
    {
        private IDoorRepository _repository;
        public Door(IDoorRepository repository)
        {
            _repository = repository;
        }

        public Building Building {  get { return _repository.GetBuilding(this); } }
    }
}

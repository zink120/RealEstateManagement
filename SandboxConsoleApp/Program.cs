using BusinessEntities.Exceptions;
using BusinessEntities.Repository;
using BusinessEntities.Repository.Interface;
using Model.DB;
using Model.DB.Interface;
using Model.Model;
using System;

namespace SandboxConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {   
                var sqLiteBase = new SqLiteBase();
                
                CreateDataTable(sqLiteBase);
                IRepository repository = new Repository(sqLiteBase);
                var buildingRecord = new BuildingRecord { Name = "10e avenue" };
                repository.Building.Save(buildingRecord);

                repository.Door.Save(new DoorRecord
                                     {
                                        BuildingID = 5,//buildingRecord.BuildingID,
                                        Address = "5545, 10e avenue, Montréal, H1Y 2G9"
                                     });

                foreach (var res in repository.Building.GetAll())
                    Console.WriteLine(string.Format("BuildingID: {0}, Name: {1}, LastModifiedDate: {2}",
                                                    res.BuildingID,
                                                    res.Name,
                                                    res.LastModifiedDate));
                foreach (var res in repository.Door.GetAll())
                    Console.WriteLine(string.Format("DoorID: {0}, BuildingID: {1}, BuildingName:{2}, Adresse{3}, LastModifiedDate: {4}",
                                                    res.DoorID,
                                                    res.BuildingID,
                                                    res.Building.Name,
                                                    res.Address,
                                                    res.LastModifiedDate));
            }
            catch (IdNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void CreateDataTable(IDb db)
        {
            IModelFactory modelFactory = new ModelFactory(db);
            modelFactory.CreateDataTable();
        }
    }
}

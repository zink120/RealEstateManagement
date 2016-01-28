using BusinessEntities.Exceptions;
using BusinessEntities.Repository;
using BusinessEntities.Repository.Interface;
using BusinessEntities.Repository.Record;
using Model.DB;
using Model.DB.Interface;
using Model.Model;
using Model.Model.Dao;
using System;

namespace SandboxConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                IDb sqLiteBase = new SqLiteBase();
                IModelFactory modelFactor = new ModelFactory(sqLiteBase);
                IRepository repository = new Repository(modelFactor);

                modelFactor.DropDataTable();
                modelFactor.CreateDataTable();
                InsertFakeData(repository);
                PrintResult(repository);
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

        private static void PrintResult(IRepository repository)
        {
            foreach (IBuilding res in repository.Building.GetAll())
            {
                Console.WriteLine($"BuildingID: {res.BuildingID}, Name: {res.Name}, LastModifiedDate: {res.LastModifiedDate}");

                foreach (IDoor door in res.Doors)
                {
                    Console.WriteLine($"DoorID: {door.DoorID}, BuildingID: {door.BuildingID}, Adresse{door.Address}, LastModifiedDate: {door.LastModifiedDate}");
                    foreach (ITenant tenant in door.Tenants)
                    {
                        Console.WriteLine($"TenantID: {tenant.TenantID}, Name: {tenant.FirstName} {tenant.LastName}, LastModifiedDate: {tenant.LastModifiedDate}");

                        foreach (ITenantInteraction tenantInteraction in tenant.TenantInteractions)
                        {
                            Console.WriteLine($"TenantInteractionOD: {tenantInteraction.TenantInteractionID}, AsOfDate: {tenantInteraction.AsOfDate}, Comment: {tenantInteraction.Comment}, LastModifiedDate: {tenantInteraction.LastModifiedDate}");
                        }
                    }
                }
            }
           
        }

        private static void InsertFakeData(IRepository repository)
        {
            var buildingRecord = new BuildingRecord { Name = "10e avenue" };
            repository.Building.Save(buildingRecord);

            var door5545 = new DoorRecord
            {
                BuildingID = buildingRecord.BuildingID,
                Address = "5545, 10e avenue, Montréal, H1Y 2G9"
            };
            repository.Door.Save(door5545);

            var thomasMorel = new TenantRecord()
            {
                DoorID = door5545.DoorID,
                FirstName = "Thomas",
                LastName = "Morel"
            };

            repository.Tenant.Save(thomasMorel);

            var tenantInteractionRecord = new TenantInteractionRecord()
            {
                AsOfDate = DateTime.Today,
                Comment = "Remise de son releve 31",
                TenantID = thomasMorel.TenantID
            };

            repository.TenantInteraction.Save(tenantInteractionRecord);
        }
    }
}

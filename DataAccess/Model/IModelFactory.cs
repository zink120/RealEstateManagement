using Model.Model.Dao;

namespace Model.Model
{
    public interface IModelFactory
    {
        IBuildingDao Building { get; }
        IDoorDao Door { get; }
        ITenantDao Tenant { get; }
        ITenantInteractionDao TenantInteraction { get; }
        IInvoiceDao Invoice { get; }

        /// <summary>
        /// Permet de créer les divers tables nécessaire au bon fonctionnement de la BD.
        /// Si les tables existent, elles seront recréées sans données. (Les données seront effacées.)
        /// </summary>
        void CreateDataTable();

        void DropDataTable();
    }
}

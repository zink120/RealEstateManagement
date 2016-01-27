namespace BusinessEntities.Repository.Interface
{
    public interface IRepository
    {
        IBuildingRepository Building { get;}
        IDoorRepository Door { get; }
        ITenantRepository Tenant { get; }
    }
}

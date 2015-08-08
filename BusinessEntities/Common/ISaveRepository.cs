namespace BusinessEntities.Common
{
    public interface ISaveRepository<T>
    {
        void Save(T record);
    }
}

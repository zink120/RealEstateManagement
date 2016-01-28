using System;

namespace BusinessEntities.Repository.Record
{
    public interface ITenantInteraction
    {
        int TenantInteractionID { get; }
        int TenantID { get; }
        DateTime AsOfDate { get; }
        string Comment { get; }
        DateTime LastModifiedDate { get; }
        
    }
}
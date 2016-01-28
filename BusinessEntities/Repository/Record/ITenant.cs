using System;
using System.Collections;
using System.Collections.Generic;

namespace BusinessEntities.Repository.Record
{
    public interface ITenant
    {
        int TenantID { get; }
        int DoorID { get; }
        string FirstName { get; }
        string LastName { get; }
        DateTime LastModifiedDate { get; }

        IEnumerable<ITenantInteraction> TenantInteractions { get; }
    }
}
using System;

namespace BusinessEntities.Exceptions
{
    public class IdNotFoundException : Exception
    {
        public IdNotFoundException(int id, string className, string methodName)
            :base($"{className}.{methodName} : Impossible de trouver l'id {id}")
        {}
    }
}

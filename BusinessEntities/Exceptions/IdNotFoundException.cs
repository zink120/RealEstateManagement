using System;

namespace BusinessEntities.Exceptions
{
    public class IdNotFoundException : Exception
    {
        public IdNotFoundException(int id, string className, string methodName)
            :base(string.Format("{0}.{1} : Impossible de trouver l'id {2}", className, methodName, id))
        {}
    }
}

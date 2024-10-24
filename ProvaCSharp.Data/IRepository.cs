using ProvaCSharp.Data.Util;
using System.Collections.Generic;

namespace ProvaCSharp.Data
{
    public interface IRepository<T>
    {        
        Retorno Save(List<T> item);
    }
}

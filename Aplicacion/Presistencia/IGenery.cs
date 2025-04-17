using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Persistencia
{
    public interface IGenery<T>
    {
       T Get(string id);
       List<T> Get();

       T Insert(T item);
       bool Update(T item);
       bool Delete(string id);
       

    }
}

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

       T Insert(T item,int id=0);
       T Update(T item);
       bool Delete(string id);
       bool InsertLogs(T item);

    }
}

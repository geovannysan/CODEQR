using Aplicacion.Persistencia;
using Entity;
using NEWCODES.Aplicacion.Presistencia;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class DispositivoLocalidadRepositoriy : IDispositivoLocalidad
    {
        EevntoContext _dbcontext = new EevntoContext();
        public bool Delete(string id)
        {
            if(int.Parse(id)==0) return false;
            var nuevo = _dbcontext.DispositivoLocation.Find(int.Parse(id));
            _dbcontext.DispositivoLocation.Remove(nuevo);
            _dbcontext.SaveChanges();
            return true;
        }

        public DispositivoLocation Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<DispositivoLocation> Get()
        {
            throw new NotImplementedException();
        }

        public DispositivoLocation Insert(DispositivoLocation item, int id = 0)
        {
            _dbcontext.DispositivoLocation.Add(item);
            _dbcontext.SaveChanges();
            return item;
        }

        public bool InsertLogs(DispositivoLocation item)
        {
            throw new NotImplementedException();
        }

        public bool Update(DispositivoLocation item)
        {
            throw new NotImplementedException();
        }

        DispositivoLocation IGenery<DispositivoLocation>.Update(DispositivoLocation item)
        {
            throw new NotImplementedException();
        }
    }
}

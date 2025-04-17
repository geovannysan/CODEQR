using Entity;
using NEWCODES.Aplicacion.Presistencia;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class DispoditivosRepsoitory : IDispoitivoRepository
    {
        private readonly EevntoContext _context;
        public DispoditivosRepsoitory(EevntoContext context)
        {
            _context = context;
        }

        public bool Delete(string id)
        {
            var dispo= _context.Dispositivos.Find(Convert.ToInt32(id));
            if (dispo != null)
            {
                _context.Remove(dispo);
                return true;
            }
            return false;
           //     throw new NotImplementedException();
        }

        public Dispositivos Get(string id)
        {
            var dispo = _context.Dispositivos.Find(Convert.ToInt32(id));
            return dispo;
        }

        public List<Dispositivos> Get()
        {
            throw new NotImplementedException();
        }

        public Dispositivos Insert(Dispositivos item)
        {
           _context.Dispositivos.Add(item);
           _context.SaveChanges();
            return item;

        }
        public  DispositivoLocation InserTDispovos(DispositivoLocation dispositivo)
        {
            _context.DispositivoLocation.Add(dispositivo);
            _context.SaveChanges();
            return dispositivo;
        }

        public bool Update(Dispositivos item)
        {
            throw new NotImplementedException();
        }
    }
}

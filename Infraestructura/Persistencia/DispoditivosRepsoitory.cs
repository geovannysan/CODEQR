using Aplicacion.Persistencia;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entity;
using Microsoft.EntityFrameworkCore;
using NEWCODES.Aplicacion.Presistencia;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class DispoditivosRepsoitory : IDispoitivoRepository
    {
        EevntoContext _context = new EevntoContext();
  

        public bool Delete(string id)
        {
            var dispo= _context.Dispositivos.Find(Convert.ToInt32(id));
            if (dispo != null)
            {
                _context.Remove(dispo);
                _context.SaveChanges();
                return true;
            }
            return false;
           //     throw new NotImplementedException();
        }

        public Dispositivos Get(string id)
        {
            var dispo = _context.Dispositivos.AsNoTracking().First();
            return dispo;
        }
        public Dispositivos GetUnic(string id, int evento)
        {
            var dispo = _context.Dispositivos.AsNoTracking().Where(x => x.EventoID == evento && x.IDequipo == id).FirstOrDefault();
            return dispo;
        }


        public List<Dispositivos> Get()
        {

            throw new NotImplementedException();
        }
        public List<Dispositivos> Getlist(int id)
        {
            return _context.Dispositivos.AsNoTracking().Where(x=>x.EventoID.Equals(id)).ToList();

        }

        public Dispositivos Insert(Dispositivos item,int id)
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
            _context.Dispositivos.Update(item);
            _context.SaveChanges();
            return true;
        }

        public bool InsertLogs(Dispositivos item)
        {
            throw new NotImplementedException();
        }

        Dispositivos IGenery<Dispositivos>.Update(Dispositivos item)
        {
            throw new NotImplementedException();
        }
    }
}

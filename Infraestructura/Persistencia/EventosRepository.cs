using Aplicacion.Persistencia;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class EventosRepository:IEventRepsoitory
    {
        EevntoContext _dbContext = new EevntoContext();
        public bool Add(Eventos student)
        {
            _dbContext.Eventos.Add(student);
            return _dbContext.SaveChanges() > 0;
        }

        public bool Delete(string id)
        {
            var even = _dbContext.Set<Eventos>().AsNoTracking().SingleOrDefault(x=>x.Id.Equals(Convert.ToInt32(id)));
            if (even == null)
            {
                return false;
            }
            else
            {
                _dbContext.Eventos.Remove(even);
                _dbContext.SaveChanges();
                return true;
            }
            throw new NotImplementedException();
        }

        public Eventos Get(string id)
        {
            var even = _dbContext.Set<Eventos>().AsNoTracking().SingleOrDefault(x => x.Id.Equals(Convert.ToInt32(id)));
            
             return even;
            
        }

        public List<Eventos> Get()
        {
            throw new NotImplementedException();
        }

        public List<Eventos> GetAll()
        {
            try
            {
                return _dbContext.Eventos.ToList();
            }
            catch (Exception ex) { 
             Console.WriteLine(ex.ToString());
                throw ;
            }
           
        }

        public Eventos Insert(Eventos item, int id)
        {
            throw new NotImplementedException();
        }

        public bool InsertLogs(Eventos item)
        {
            throw new NotImplementedException();
        }

        public Eventos Update(Eventos item)
        {
            _dbContext.Eventos.Update(item);
            _dbContext.SaveChanges();
            return item;
        }
    }
}

using Aplicacion.Persistencia;
using Microsoft.EntityFrameworkCore;
using NEWCODES.Aplicacion.Presistencia;
using NEWCODES.Domain.Entity;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class LogsEvenRepository : ILogsEvents
    {
        EevntoContext _context = new EevntoContext();
        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public LogsEventos Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<LogsEventos> Get()
        {
            throw new NotImplementedException();
        }

        public LogsEventos Insert(LogsEventos item, int id = 0)
        {
            throw new NotImplementedException();
        }
        public List<LogsEventos> ObtenerLista( int id)
        {
             return _context.Set<LogsEventos>().AsNoTracking().Where(x=>x.IdEvento.Equals(id)).ToList();
        }


        public bool InsertLogs(LogsEventos item)
        {
            throw new NotImplementedException();
        }

        public bool Update(LogsEventos item)
        {
            throw new NotImplementedException();
        }

        LogsEventos IGenery<LogsEventos>.Update(LogsEventos item)
        {
            throw new NotImplementedException();
        }
    }
}

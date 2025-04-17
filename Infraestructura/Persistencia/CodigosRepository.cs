using Entity;
using Microsoft.EntityFrameworkCore;
using NEWCODES.Aplicacion.Presistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class CodigosRepository : ICodigosRepository
    {
        EevntoContext _context = new EevntoContext();
       

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Codigos Get(string id)
        {
            return _context.Set<Codigos>().AsNoTracking().FirstOrDefault(x => x.EventoID.Equals(Convert.ToInt32(id)));
        }

        public List<Codigos> Get()
        {
           return _context.Set<Codigos>().AsNoTracking().ToList();
        }

        public Codigos Insert(Codigos item)
        {
            var codigos = _context.Set<Codigos>().AsNoTracking().FirstOrDefault(x=>x.Codigo == item.Codigo);

            if (codigos == null)
            {
                _context.Codigos.Add(item);
                _context.SaveChanges();
                var localdiad = _context.Set<Localidades>().AsNoTracking().Where(x=>x.IdEvento.Equals(item.EventoID)).Where(x=>x.Name.Equals(item.Name)).FirstOrDefault();
                if (localdiad == null)
                {
                    _context.Localidades.Add(new Localidades
                    {
                        Name = item.Name,
                        IdEvento= item.EventoID,
                    });
                    _context.SaveChanges();
                }
                else
                {
                    var localidads = new Localidades
                    {
                        Name = localdiad.Name,
                        IdEvento = localdiad.IdEvento,
                        Count = "0",
                    };
                    _context.Localidades.Update(localidads);
                    _context.SaveChanges();

                }
                return item;
            }
            else
            {
                return codigos;
            }

           // throw new NotImplementedException();
        }

        public bool Update(Codigos item)
        {
            throw new NotImplementedException();
        }
    }
}

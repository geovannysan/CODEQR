using Entity;
using Microsoft.EntityFrameworkCore;
using NEWCODES.Aplicacion.DTO;
using NEWCODES.Aplicacion.Presistencia;
using NEWCODES.Domain.Entity;

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
        public List<Codigos> GetList(string id)
        {
            return _context.Set<Codigos>().AsNoTracking().Where(x => x.EventoID.Equals(Convert.ToInt32(id))).ToList();
        }
        public List<Codigos> Get()
        {
           return _context.Set<Codigos>().AsNoTracking().ToList();
        }

        public Codigos Insert(Codigos item, int id)
        {
            var codigos = _context.Set<Codigos>().AsNoTracking().FirstOrDefault(x=>x.Codigo == item.Codigo && x.EventoID ==item.EventoID);
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
                return item;
            }
            else
            {
                return null;
            }
        }
        public MessageSocket Verifica(string id,string idevent,MessageSocket message)
        {
            var localdiad = _context.Set<Codigos>().AsNoTracking().Where(x => x.Codigo == message.Codigo && x.EventoID == int.Parse( idevent)).Select(x => new{x.Id, x.Codigo,x.Name,x.Precio,x.Estado,x.EventoID,time = DateTime.Now - x.time,x.info}).FirstOrDefault();
            if (localdiad == null)
            {              
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "Rechazado", Mensaje = $"Código {message.Codigo} no encontrado", Tipo = "Codigo", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"Código {message.Codigo} no encontrado" ,Type="Error"};
            }           
            var lsita = _context.Set<Localidades>().AsNoTracking().Where(x => x.Name == localdiad.Name && x.IdEvento == int.Parse(idevent)).FirstOrDefault();
            if (lsita == null) {
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "No encontrado", Mensaje = $"Localidad {lsita.Name} no agregada", Tipo = "Localidad", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"Localidad {localdiad.Name} no ha sido Agregado al evento", Type = "Error" };
            }
            var dispo = _context.Set<DispositivoLocation>().AsNoTracking().Where(x=>x.LocalidadID == lsita.Id  && x.DispoId== int.Parse( id)).FirstOrDefault();
            if (dispo == null) {
                var dipoid = _context.Set<Dispositivos>().AsNoTracking().Where(x => x.Id == int.Parse(id)).FirstOrDefault();
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "Rechazado", Mensaje = $"Código {message.Codigo} no autorizado para equipo {dipoid.Name}", Tipo = "Codigo", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"No Autorizado para el escaneo en este Dispositivo", Type = "Error" };
            }
            if (localdiad.Estado.Equals("Scaneado"))
            {
                 TimeSpan diff = localdiad.time;
                string formateado = $"{(int)diff.TotalDays} Días, {diff.Hours} HH. {diff.Minutes} min. {diff.Seconds} seg.";
                Console.WriteLine(formateado);
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "Repetido", Mensaje = $"Código {message.Codigo} ya fue escaneado", Tipo = "Codigo", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"Codigo escaneado\n {formateado}", Type = "Error" };
            }
            var codigo = new Codigos { Id = localdiad.Id };
            _context.Attach(codigo);
            codigo.time = DateTime.Now;
            codigo.Estado = "Scaneado";
            _context.Entry(codigo).Property(x => x.time).IsModified = true;
            _context.SaveChanges();
            return new MessageSocket { Codigo = $"Válido", Type = "Success" };
        }

        public MessageSocket VerificaSalida(string id, string idevent, MessageSocket message)
        {
            var localdiad = _context.Set<Codigos>().AsNoTracking().Where(x => x.Codigo == message.Codigo && x.EventoID == int.Parse(idevent)).Select(x => new { x.Id, x.Codigo, x.Name, x.Precio, x.Estado, x.EventoID, time = DateTime.Now - x.time, x.info }).FirstOrDefault();
            if (localdiad == null)
            {
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "Rechazado", Mensaje = $"Código {message.Codigo} no encontrado", Tipo = "Codigo", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"Código {message.Codigo} no encontrado", Type = "Error" };
            }
            var lsita = _context.Set<Localidades>().AsNoTracking().Where(x => x.Name == localdiad.Name && x.IdEvento == int.Parse(idevent)).FirstOrDefault();
            if (lsita == null)
            {
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "No encontrado", Mensaje = $"Localidad {lsita.Name} no agregada", Tipo = "Localidad", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"Localidad {localdiad.Name} no ha sido Agregado al evento", Type = "Error" };
            }
            var dispo = _context.Set<DispositivoLocation>().AsNoTracking().Where(x => x.LocalidadID == lsita.Id && x.DispoId == int.Parse(id)).FirstOrDefault();
            if (dispo == null)
            {
                var dipoid = _context.Set<Dispositivos>().AsNoTracking().Where(x => x.Id == int.Parse(id)).FirstOrDefault();
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "Rechazado", Mensaje = $"Código {message.Codigo} no autorizado para equipo {dipoid.Name}", Tipo = "Codigo", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"No Autorizado para el escaneo en este Dispositivo", Type = "Error" };
            }
            if (localdiad.Estado.Equals("Scaneado"))
            {
                TimeSpan diff = localdiad.time;
                string formateado = $"{(int)diff.TotalDays} Días, {diff.Hours} HH. {diff.Minutes} min. {diff.Seconds} seg.";
                Console.WriteLine(formateado);
                _context.Add(new LogsEventos { Codigo = message.Codigo, Estado = "Sale", Mensaje = $"Código {message.Codigo} Sale", Tipo = "Codigo", time = DateTime.Now, IdEvento = int.Parse(idevent) });
                _context.SaveChanges();
                return new MessageSocket { Codigo = $"Codigo Sale\n {formateado}", Type = "Error" };
            }
            var codigo = new Codigos { Id = localdiad.Id };
            _context.Attach(codigo);
            codigo.time = DateTime.Now;
            codigo.Estado = "";
            _context.Entry(codigo).Property(x => x.time).IsModified = true;
            _context.SaveChanges();
            return new MessageSocket { Codigo = $"Válido", Type = "Success" };
        }
        public bool Update(Codigos item)
        {
            throw new NotImplementedException();
        }

        public bool InsertLogs(Codigos item)
        {
            throw new NotImplementedException();
        }
    }
}

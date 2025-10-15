using Aplicacion.Persistencia;
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


        public async Task InsertBatchAsync(IEnumerable<Codigos> nuevosCodigos)
        {
            // 🔹 Extrae las combinaciones únicas que ya existen
            var eventoIds = nuevosCodigos.Select(c => c.EventoID).Distinct().ToList();
            var codigosExistentes = await _context.Codigos
                .Where(c => eventoIds.Contains(c.EventoID))
                .Select(c => new { c.Codigo, c.EventoID })
                .ToListAsync();

            var existentesSet = new HashSet<(string Codigo, int EventoID)>(
                codigosExistentes.Select(c => (c.Codigo, c.EventoID))
            );

            // 🔹 Filtra solo los nuevos
            var nuevosUnicos = nuevosCodigos
                .Where(c => !existentesSet.Contains((c.Codigo, c.EventoID)))
                .ToList();

            if (nuevosUnicos.Count == 0)
                return;

            const int batchSize = 1000;
            for (int i = 0; i < nuevosUnicos.Count; i += batchSize)
            {
                var lote = nuevosUnicos.Skip(i).Take(batchSize);
                await _context.Codigos.AddRangeAsync(lote);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Codigos>> GetListAsync(int eventoId)
        {
            return await _context.Codigos
                .Where(c => c.EventoID == eventoId)
                .ToListAsync();
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
        public MessageSocket Verifica(string id, string idevent, MessageSocket message)
        {
            if (string.IsNullOrEmpty(message.Codigo))
                return new MessageSocket { Codigo = "Ingrese un Código", Type = "Error" };

            if (!int.TryParse(idevent, out int eventoIdInt))
                return new MessageSocket { Codigo = "ID de evento inválido", Type = "Error" };

            if (!int.TryParse(id, out int dispositivoIdInt))
                return new MessageSocket { Codigo = "ID de dispositivo inválido", Type = "Error" };

            var db = new AdoContext();

            // Buscar código
            using var cmd = db.CreateCommand(@"
        SELECT Id, Codigo, Name, Asiento, Estado, EventoID, time, info 
        FROM Codigos 
        WHERE Codigo=@codigo AND EventoID=@eventoId
        LIMIT 1");
            cmd.Parameters.AddWithValue("@codigo", message.Codigo);
            cmd.Parameters.AddWithValue("@eventoId", eventoIdInt);

            int codigoId = 0;
            string estado = null, name = null, info = null, asiento = null;
            DateTime time = DateTime.MinValue;

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                codigoId = Convert.ToInt32(reader["Id"]);
                estado = reader["Estado"].ToString();
                name = reader["Name"].ToString();
                info = reader["info"]?.ToString();
                asiento = reader["Asiento"]?.ToString();
                if (reader["time"] != DBNull.Value)
                    time = Convert.ToDateTime(reader["time"]);
            }

            // Buscar dispositivo
            using var cmdDisp = db.CreateCommand("SELECT Name FROM Dispositivos WHERE Id=@id");
            cmdDisp.Parameters.AddWithValue("@id", dispositivoIdInt);
            var dispositivoName = cmdDisp.ExecuteScalar()?.ToString();

            if (codigoId == 0)
            {
                // Insertar código rechazado
              /*  using var insertCmd = db.CreateCommand(@"
            INSERT INTO Codigos (Name, Codigo, EventoID, Estado, info, time, Conteo, Asiento)
            VALUES ('Adicional', @codigo, @eventoId, 'Rechazado', @info, @time, 0, @asiento)");
                insertCmd.Parameters.AddWithValue("@codigo", message.Codigo);
                insertCmd.Parameters.AddWithValue("@eventoId", eventoIdInt);
                insertCmd.Parameters.AddWithValue("@info", dispositivoName != null ? dispositivoName + " - " + message.Type : message.Type);
                insertCmd.Parameters.AddWithValue("@time", DateTime.Now);
                insertCmd.Parameters.AddWithValue("@asiento", message.Codigo);
                insertCmd.ExecuteNonQuery();*/

                // Insertar log
                using var logCmd = db.CreateCommand(@"
            INSERT INTO LogsEventos (Codigo, Estado, Mensaje, Tipo, time, IdEvento)
            VALUES (@codigo, 'Rechazado', @mensaje, 'Codigo', @time, @eventoId)");
                logCmd.Parameters.AddWithValue("@codigo", message.Codigo);
                logCmd.Parameters.AddWithValue("@mensaje", $"Código {message.Codigo} no encontrado");
                logCmd.Parameters.AddWithValue("@time", DateTime.Now);

                logCmd.Parameters.AddWithValue("@eventoId", eventoIdInt);
                logCmd.ExecuteNonQuery();

                return new MessageSocket { Codigo = $"Código no encontrado {message.Codigo}", Type = "Warning" };
            }

            if (estado == "Scaneado")
            {
                TimeSpan diff = DateTime.Now - time;
                string formateado = $"{(int)diff.TotalDays} Días, {diff.Hours} HH. {diff.Minutes} min. {diff.Seconds} seg.";

                using var logCmd = db.CreateCommand(@"
            INSERT INTO LogsEventos (Codigo, Estado, Mensaje, Tipo, time, IdEvento)
            VALUES (@codigo, 'Repetido', @mensaje, 'Codigo', @time, @eventoId)");
                logCmd.Parameters.AddWithValue("@codigo", message.Codigo);
                logCmd.Parameters.AddWithValue("@mensaje", $"Código {message.Codigo} ya fue escaneado");
                logCmd.Parameters.AddWithValue("@time", DateTime.Now);
                logCmd.Parameters.AddWithValue("@eventoId", eventoIdInt);
                logCmd.ExecuteNonQuery();

                return new MessageSocket { Codigo = $"Código escaneado anteriormente - {formateado}", Type = "Error" };
            }

            // Actualizar código como escaneado
            using var updateCmd = db.CreateCommand(@"
        UPDATE Codigos 
        SET Estado='Scaneado', time=@time, info=@info
        WHERE Id=@id");
            updateCmd.Parameters.AddWithValue("@time", DateTime.Now);
            updateCmd.Parameters.AddWithValue("@info", dispositivoName != null ? dispositivoName + " - " + message.Type : message.Type);
            updateCmd.Parameters.AddWithValue("@id", codigoId);
            updateCmd.ExecuteNonQuery();

            return new MessageSocket { Codigo = "Válido", Type = "Success" };
        }

        public MessageSocket VerificaSalida(string id, string idevent, MessageSocket message)
        {
            var localdiad = _context.Set<Codigos>().AsNoTracking().Where(x => x.Codigo == message.Codigo && x.EventoID == int.Parse(idevent)).Select(x => new { x.Id, x.Codigo, x.Name, x.Asiento, x.Estado, x.EventoID, time = DateTime.Now - x.time, x.info }).FirstOrDefault();
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
                return new MessageSocket { Codigo = $"Codigo ya Escaneado - \n {formateado}", Type = "Error" };
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

        Codigos IGenery<Codigos>.Update(Codigos item)
        {
            throw new NotImplementedException();
        }
    }
}

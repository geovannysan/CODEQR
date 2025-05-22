using Aplicacion.Persistencia;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NEWCODES.Aplicacion.DTO;
using NEWCODES.Aplicacion.Presistencia;

namespace NEWCODES.Infraestructura.Persistencia
{
    public class LocalidadesRepository : ILocalidades
    {
        EevntoContext _dbContext = new EevntoContext();
        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Localidades Get(string id)
        {
            throw new NotImplementedException();
        }
        public List<DispositivoLocation> GetInfolocali(string id,string ids )
        {
            var resultado = (from localidad in _dbContext.Localidades
                             where localidad.IdEvento == int.Parse(id)
                            join codigo in _dbContext.DispositivoLocation
                           on new { LocalidadID = localidad.Id, DispoId = int.Parse(ids) }
                          equals new { codigo.LocalidadID, codigo.DispoId }
                             into grupoCodigos
                             from codigo in grupoCodigos.DefaultIfEmpty()
                        
                            select new DispositivoLocation
                            {
                                Name = localidad.Name,
                                LocalidadID = localidad.Id,
                                DispoId = codigo != null ? codigo.Id : 0
                                
                            }).ToList();

            return resultado;
        }
        public List<LocalidadCountDto> GetInfo(string id)
        {
            int eventoId = int.Parse(id);
            var conteoPorNombre = _dbContext.Codigos
                                     .Where(c => c.EventoID == eventoId)
                                     .GroupBy(c => c.Name)
                                     .Select(g => new LocalidadCountDto
                                     {
                                         Name = g.Key,
                                         Count = Convert.ToInt32(g.Count()),
                                         Scaneado = Convert.ToInt32(g.Count(x => x.Estado == "Scaneado"))
                                     }).ToList();           
            return conteoPorNombre;
        }


        public List<Localidades> Get()
        {
            throw new NotImplementedException();
        }

        public Localidades Insert(Localidades item, int id = 0)
        {
            throw new NotImplementedException();
        }

        public bool Update(Localidades item)
        {
            throw new NotImplementedException();
        }

        public bool InsertLogs(Localidades item)
        {
            throw new NotImplementedException();
        }
    }
}

using Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEWCODES.Domain.Entity
{
    public class LogsEventos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Codigo { get; set; }
         public DateTime  time { get; set; }
        public string? Estado { get; set; }
        public string? Mensaje { get; set; }
        public int IdEvento { get; set; }
        public Eventos Eventos { get; set; }
    }
}

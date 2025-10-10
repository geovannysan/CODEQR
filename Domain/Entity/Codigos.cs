using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Codigos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Codigo { get; set; }
        public string? Asiento { get; set; }
        public string? info { get; set; } 
        public string? Estado { get; set; } 
        public DateTime time { get; set; }
        public int EventoID { get; set; }
        public int? Conteo { get; set; } = 0;
        public Eventos Eventos { get; set; }  
    }
}

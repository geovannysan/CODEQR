using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Eventos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; } 
        public DateTime Fecha { get; set; }
        public string Description { get; set; } = "";
        public int SelecionLocation { get; set; } = 0;

    }
}

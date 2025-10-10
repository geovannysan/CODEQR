using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Localidades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Count { get; set; } = string.Empty;
        public int IdEvento { get; set; }
        public int TotalIngres { get; set; } = 1;

        public Eventos Eventos { get; set; }
    }
}

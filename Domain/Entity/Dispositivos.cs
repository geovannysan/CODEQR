using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Dispositivos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Name { get; set; }
        public string IDequipo { get; set; }
        public string Ip { get; set; } = "";
        public string Estado {  get; set; }
        public int EventoID { get; set; }
        public Eventos Eventos { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class DispositivoLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Localidades")]
        public int LocalidadID { get; set; }
        public Localidades Localidades { get; set; }

        [ForeignKey("Dispositivos")]
        public int DispoId { get; set; }
        public Dispositivos Dispositivos { get; set; }

    }
}

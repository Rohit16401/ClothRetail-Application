using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.Racks")]
    public class Racks
    {
        [Key]
        public string Rack_Id { get; set; }
        public string Rack_Identifier { get; set; }
        public string Section { get; set; }
        public string Column { get; set; }
        public string Sequence { get; set; }
    }
}

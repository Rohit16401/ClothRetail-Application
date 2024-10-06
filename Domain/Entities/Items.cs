using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.Items")]
    public class Items
    {
        [Key]
        public string Item_Id { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }
        public string CatL1_Id { get; set; }
        public string CatL2_Id { get; set; }
        public string CatL3_Id { get; set; }
        public string Rack_Id { get; set; }


    }
}

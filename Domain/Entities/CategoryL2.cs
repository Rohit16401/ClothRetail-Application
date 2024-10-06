using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.CategoryL2")]
    public class CategoryL2
    {
        [Key]
        public string CatL2_Id { get; set; }
        public string CatL1_Id { get; set; }
        public string Category_L2 { get; set; }
    }
}

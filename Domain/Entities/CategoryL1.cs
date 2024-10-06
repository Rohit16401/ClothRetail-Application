using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.CategoryL1")]
    public class CategoryL1
    {
        [Key]
        public string CatL1_Id { get; set; }
        public string CatL0_Id { get; set; }
        public string Category_L1 { get; set; }
    }
}

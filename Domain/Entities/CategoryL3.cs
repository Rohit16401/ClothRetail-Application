using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.CategoryL3")]
    public class CategoryL3
    {
        [Key]
        public string CatL3_Id { get; set; }
        public string CatL2_Id { get; set; }
        public string Category_L3 { get; set; }
    }
}

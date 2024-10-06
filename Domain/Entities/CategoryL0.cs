using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.CategoryL0")]
    public class CategoryL0
    {
        [Key]
        public string CatL0_Id { get; set; }
        public string Category_L0 { get; set; }
    }
}

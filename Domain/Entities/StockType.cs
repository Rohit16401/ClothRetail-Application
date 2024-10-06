using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.StockType")]
    public class StockType
    {
        [Key]
        public string StockType_Id { get; set; }
        public string Description { get; set; }

    }
}

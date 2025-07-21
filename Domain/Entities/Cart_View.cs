using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("dbo.Cart_View")]

    public class Cart_View
    {
        [Key]
        public string Item_Id { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }
        public int Qty { get; set; }
        public double GST { get; set; }
        public double MRP { get; set; }
        public double Discount { get; set; }
        public double FinalSale_Price { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.Retail_Invoice")]
    public class Retail_Invoice
    {
        [Key]
        public string Invoice_Id { get; set; }
        public string Date { get; set; }
        public string Item_Id { get; set; }
        public string Customer_Id { get; set; }
        public string Qty { get; set; }
        public string MRP { get; set; }

        public string Discount { get; set; }
        public string FinalSale_Price { get; set; }

    }
}

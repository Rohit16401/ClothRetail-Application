using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.Goods")]
    public class Goods
    {
        [Key]
        public string Goods_Id { get; set; }
        public string Item_Id { get; set; }

        public string In_Date { get; set; }
        public string Qty { get; set; }
        public string StockType_Id { get; set; }
        public string Rack_Id { get; set; }
        public string Buy_Price { get; set; }
        public string Static_Charge { get; set; }
        public string GST { get; set; }
        public string MRP { get; set; }
        public string Discount { get; set; }
        public string FinalSale_Price { get; set; }
    }
}

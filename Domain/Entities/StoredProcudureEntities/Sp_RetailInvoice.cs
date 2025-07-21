using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.StoredProcudureEntities
{
    [Table("dbo.InsertRetailInvoice")]
    public class Sp_RetailInvoice
    {
      
        public DateTime Date { get; set; }
      //  public string Item_Id { get; set; }
        public string PhoneNo { get; set; }
        public string Type { get; set; }
        public double GrandTotal { get; set; }
        //public string MRP { get; set; }

       public double Discount { get; set; }
        //public string FinalSale_Price { get; set; }
    }
}

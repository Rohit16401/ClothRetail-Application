using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.StoredProcudureEntities
{
    [Table("dbo.InsertWholeInvoice")]
    public class Sp_WholeSaleInvoice
    {
       
        public DateTime Date { get; set; }
       // public string Item_Id { get; set; }
        public string PhoneNo { get; set; }
        public string Type { get; set; }
      //  public string MRP { get; set; }
        public double Discount { get; set; }
        public string GST { get; set; }
       // public string Sale_Price { get; set; }
        public double Grand_Total { get; set; }

    }
}

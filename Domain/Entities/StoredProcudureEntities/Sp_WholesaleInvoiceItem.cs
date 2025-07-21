using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.StoredProcudureEntities
{
    [Table("dbo.InsertWholesaleInvoiceItem")]
    public class Sp_WholesaleInvoiceItem
    {
        public string Invoice_Id { get; set; }
        public string Item_Id { get; set; }
        public int Qty { get; set; }
        public double MRP { get; set; }
        public double SubDiscount { get; set; }
        public double Final_Price { get; set; }


    }
}

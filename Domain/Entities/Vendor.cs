using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("BT_Masters.Vendor")]
    public class Vendor
    {
        public string Phone_No { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string City { get; set; } 
        
    }
}

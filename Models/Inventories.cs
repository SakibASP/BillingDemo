using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingDemo.Models
{
    [Table("Inventories")]
    public class Inventories
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Bill No.")]
        public string BillNo { get; set; }
        [DisplayName("Customer Name")]
        public int CustomerId { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalBillAmount { get; set; }
        public double DueAmount { get; set; }
        public double PaidAmount { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customers _Customers { get; set; }
    }
}

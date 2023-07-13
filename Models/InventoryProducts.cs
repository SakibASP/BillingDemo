using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BillingDemo.Models
{
    [Table("InventoryProducts")]
    public class InventoryProducts
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public double Rate { get; set; }
        public double Qty { get; set; }
        public double Discount { get; set; }

        [ForeignKey(nameof(InventoryId))]
        public virtual Inventories _Inventories { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Products _Products { get; set; }
    }
}

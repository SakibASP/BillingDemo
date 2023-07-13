using BillingDemo.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillingDemo.ViewModels
{
    public class InventoryViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string BillNo { get; set; }
        public int InventoryId { get; set; }
        public int CustomerId { get; set; }

        [DisplayName("Products")]
        public string ProductName { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalBillAmount { get; set; }
        public double DueAmount { get; set; }
        public double PaidAmount { get; set; }
        public  List<InventoryProductsViewModel>? _InventoryProductsList { get; set; }
    }
}

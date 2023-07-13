namespace BillingDemo.ViewModels
{
    public class InventoryProductsViewModel
    {
        public int DetailId { get; set; }
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public double Rate { get; set; }
        public double Qty { get; set; }
        public double Discount { get; set; }
        public string ProductName { get; set; }
    }
}

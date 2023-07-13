using BillingDemo.Models;
using BillingDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Diagnostics;

namespace BillingDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BilllingDbContext db;

        public HomeController(ILogger<HomeController> logger, BilllingDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Bills()
        {
            return db.Inventories != null ?
                        View(await db.Inventories.Include(x=>x._Customers).ToListAsync()) :
                        Problem("Entity set 'BilllingDbContext.Inventories'  is null.");
        }

        public IActionResult BillAdd()
        {
            ViewBag.Customers = new SelectList(db.Customers, "Id", "Name");
            ViewBag.Products = new SelectList(db.Products, "Id", "Name");
            var _InventoryViewModel = new InventoryViewModel() { Date = DateTime.Today };
            return View(_InventoryViewModel);
        }

        [HttpGet]
        public JsonResult GetProduct(int productId)
        {
            var prod = db.Products.Where(x=>x.Id==productId).FirstOrDefault();
            if (prod is null)
            {
                return Json(null);
            }
            return Json(prod);
        }

        [HttpGet]
        public JsonResult GetBills(string BillNo)
        {
            if (!string.IsNullOrEmpty(BillNo))
            {
                var inv = db.Inventories.Include(x => x._Customers).Where(x => x.BillNo == BillNo).FirstOrDefault();
                if(inv is not null)
                {
                    //Populating InventoryProductViewModel Class
                    var prodList = (from invProd in db.InventoryProducts.Include(x => x._Products)
                                    where invProd.InventoryId == inv.Id
                                    select new InventoryProductsViewModel()
                                    {
                                        DetailId = invProd.Id,
                                        InventoryId = invProd.InventoryId,
                                        ProductId = invProd.ProductId,
                                        Rate = invProd.Rate,
                                        Qty = invProd.Qty,
                                        Discount = invProd.Discount,
                                        ProductName = invProd._Products.Name
                                    }).ToList();
                    var InventoryVm = new InventoryViewModel
                    {
                        InventoryId = inv.Id,
                        BillNo = inv.BillNo,
                        Date = inv.Date,
                        TotalBillAmount = inv.TotalBillAmount,
                        TotalDiscount = inv.TotalDiscount,
                        PaidAmount = inv.PaidAmount,
                        DueAmount = inv.DueAmount,
                        CustomerId = inv.CustomerId,
                        _InventoryProductsList = prodList
                    };
                    return Json(InventoryVm);
                }
                else { return Json(null); }
            }
            else { return Json(null); }
        }

        [HttpPost]
        public JsonResult BillAdd(InventoryViewModel objInventoryViewModel)
        {
            try
            {
                using (BilllingDbContext dc = new())
                {
                    //Used IsolationLevel.ReadUncommitted for avoiding deadlocks
                    using (var transaction = dc.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        return SaveBills(dc, objInventoryViewModel, transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "An unknown error has occured" });
            }
        }

        public JsonResult SaveBills(BilllingDbContext dc, InventoryViewModel objInventory, IDbContextTransaction transaction)
        {
            //Adding new inventory
            var inventories = new Inventories()
            {
                Id = objInventory.InventoryId,
                Date = objInventory.Date,
                BillNo = objInventory.BillNo,
                CustomerId = objInventory.CustomerId,
                TotalBillAmount = objInventory.TotalBillAmount,
                TotalDiscount = objInventory.TotalDiscount,
                DueAmount = objInventory.DueAmount,
                PaidAmount = objInventory.PaidAmount
            };
            dc.Inventories.Add(inventories);
            dc.SaveChanges();

            if (objInventory._InventoryProductsList.Any())
            {
                foreach (var item in objInventory._InventoryProductsList)
                {
                    var inventoryProducts = new InventoryProducts()
                    {
                        Id = item.DetailId,
                        InventoryId = inventories.Id,
                        ProductId = item.ProductId,
                        Rate = item.Rate,
                        Qty = item.Qty,
                        Discount = item.Discount
                    };

                    dc.InventoryProducts.Add(inventoryProducts);

                }
            }

            try
            {
                if (dc.SaveChanges() > 0)
                {
                    transaction.Commit();
                    return Json(new
                    {
                        data = objInventory,
                        status = true,
                        message = "Saved Successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(new { status = false, message = "An unknown error has occured" });
            }
            return Json(new { status = false, message = "An unknown error has occured" });
        }

        [HttpPost]
        public JsonResult BillUpdate(InventoryViewModel objInventoryViewModel)
        {
            try
            {
                using (BilllingDbContext dc = new())
                {
                    //Used IsolationLevel.ReadUncommitted for avoiding deadlocks
                    using (var transaction = dc.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        return UpdateBills(dc, objInventoryViewModel, transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "An unknown error has occured" });
            }
        }
        public JsonResult UpdateBills(BilllingDbContext dc, InventoryViewModel objInventory, IDbContextTransaction transaction)
        {
            //Updating inventory
            Inventories inventories = new Inventories();
            inventories.Id = objInventory.InventoryId;
            inventories.Date = objInventory.Date;
            inventories.BillNo = objInventory.BillNo;
            inventories.CustomerId = objInventory.CustomerId;
            inventories.TotalDiscount = objInventory.TotalDiscount;
            inventories.TotalBillAmount = objInventory.TotalBillAmount;
            inventories.DueAmount = objInventory.DueAmount;
            inventories.PaidAmount = objInventory.PaidAmount;
            dc.Update(inventories);
            dc.SaveChanges();

            if (objInventory._InventoryProductsList.Any())
            {
                foreach (var item in objInventory._InventoryProductsList)
                {
                    InventoryProducts model = new InventoryProducts();
                    if (item.DetailId > 0)
                    {
                        //Editing Existing Details
                        model.Id = item.DetailId;
                        model.InventoryId = objInventory.InventoryId;
                        model.ProductId = item.ProductId;
                        model.Rate = item.Rate;
                        model.Qty = item.Qty;
                        model.Discount = item.Discount;
                        dc.Update(model);
                    }
                    else
                    {
                        //Adding New Details
                        model.InventoryId = objInventory.InventoryId;
                        model.ProductId = item.ProductId;
                        model.Rate = item.Rate;
                        model.Qty = item.Qty;
                        model.Discount = item.Discount;
                        dc.InventoryProducts.Add(model);
                    }
                }
            }

            try
            {
                if (dc.SaveChanges() > 0)
                {
                    transaction.Commit();
                    return Json(new
                    {
                        data = objInventory,
                        status = true,
                        message = "Updated Successfully"
                    });
                    
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(new { status = false, message = "An unknown error has occured" });
            }

            return Json(new { status = false, message = "An unknown error has occured" });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
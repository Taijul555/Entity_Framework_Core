using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntittFramework_Project.Models;
using EntittFramework_Project.ViewModels;
using EntittFramework_Project.ViewModels.Input;

namespace EntittFramework_Project.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IWebHostEnvironment env;
        public ProductsController(ProductDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env; 
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }
        public async Task<IActionResult> Aggregates() {
            var data = await db.Sales.Include(x => x.Product)
                .ToListAsync();
            return View(data);
        }
        public IActionResult Grouping() 
        { 
            return View();
        }
        [HttpPost]
        public  IActionResult Grouping(string groupby)
        {
            
            if(groupby == "productname")
            {
                var data = db.Sales.Include(x => x.Product)
               .ToList()
               .GroupBy(x => x.Product?.ProductName)
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();
                
                return View("GroupingResult", data);
            }
            if (groupby == "year month")
            {
                var data = db.Sales.Include(x => x.Product)
                    .OrderByDescending(x=> x.Date)
               .ToList()
               .GroupBy(x => $"{x.Date:MMM, yyyy}")
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data);
            }
            if (groupby == "count")
            {
                var data = db.Sales.Include(x => x.Product)
                    .OrderByDescending(x => x.Date)
               .ToList()
               .GroupBy(x => x.Product?.ProductName)
               .Select(g => new GroupedDataPrimitve<int> { Key = g.Key, Data = g.Count() })
               .ToList();

                return View("GroupingResultPrimitive", data);
            }

            return RedirectToAction("Grouping");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if(ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName= model.ProductName,
                    Price = model.Price,
                    Size = model.Size,
                    OnSale = model.OnSale
                };
                string ext = Path.GetExtension(model.Picture.FileName);
                string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
                FileStream fs = new FileStream(savePath, FileMode.Create);
                await model.Picture.CopyToAsync(fs);
                product.Picture = fileName;
                fs.Close();
                db.Database.ExecuteSqlInterpolated($"EXEC InsertProduct {product.ProductName}, {product.Price}, {(int)product.Size}, {product.Picture}, {(model.OnSale ? 1 : 0)}");
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await db.Products.FirstOrDefaultAsync(x=> x.ProductId == id);
            if (data == null) return NotFound();
            return View(new ProductEditModel { 
                ProductId=data.ProductId,
                ProductName=data.ProductName,
                Price=data.Price,
                Size=data.Size ?? Size.S,
                //OnSale = data.OnSale ?? false

            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await db.Products.FirstOrDefaultAsync(x => x.ProductId == model.ProductId);
                if(product == null) return NotFound();
                    product.ProductId = model.ProductId;
                    product.ProductName = model.ProductName;
                    product.Price = model.Price;
                    product.Size = model.Size;
                    product.OnSale = model.OnSale;
               
                if(model.Picture != null)
                {
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    product.Picture = fileName;
                    fs.Close();
                }
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateProduct {product.ProductId}, {product.ProductName}, {product.Price}, {(int)product.Size}, {product.Picture}, {(model.OnSale ? 1 : 0)}");
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public  IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteProduct {id}");
            return Json(new { success=true, id });
        }
    }
}

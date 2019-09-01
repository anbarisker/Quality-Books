using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBooks.Data;
using QualityBooks.Models;
using System.Net.Http.Headers; //Week 6
using Microsoft.AspNetCore.Hosting; //Week 6
using Microsoft.AspNetCore.Http; //Week 6
using System.IO; //Week 6
using System.Drawing;

namespace QualityBooks.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly QualityBooksContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        public AdminProductsController(QualityBooksContext context, IHostingEnvironment hEnv)
        {
            _context = context;
            _hostingEnv = hEnv;
        }

        // GET: AdminProducts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(c => c.Category)
                .Include(s => s.Supplier).ToListAsync());
        }

        // GET: AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(c => c.Category)
                .Include(s => s.Supplier)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: AdminProducts/Create
        public IActionResult Create()
        {
            PopulateCategoryDropDown(4);
            PopulateSupplierDropDown();
            return View();
        }

        private void PopulateCategoryDropDown(object selectCategory = null)
        {
            var categoriesQuery = from d in _context.Categories select d;
            ViewBag.CategoryId = new SelectList(categoriesQuery.AsNoTracking(), "Id", "CategroyName", selectCategory);
            
        }

        private void PopulateSupplierDropDown(object selectSupplier = null)
        {
            var suppliersQuery = from d in _context.Suppliers
                                 orderby d.FirstName
                                 select d;
            ViewBag.SupplierId = new SelectList(suppliersQuery.AsNoTracking(), "Id", "FirstName", selectSupplier);
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,SupplierId,ProductName,ProductPrice, Description")] Product product, IFormFile _files)
        {
            /*
            var relativeName = "";
            var fileName = "";

            if (_files.Count < 1)
            {
                relativeName = "/Images/logo.jpg";
            }
            else
            {
                foreach (var file in _files)
                {
                    fileName = ContentDispositionHeaderValue
                                      .Parse(file.ContentDisposition)
                                      .FileName
                                      .Trim('"');
                    //Path for localhost
                    relativeName = "/ProductImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                    using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
                product.ProductImage = relativeName;
            }
            */

            

            try
            {
                if (ModelState.IsValid)
                {
                    if (_files != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await _files.CopyToAsync(memoryStream);
                            product.ProductImage = memoryStream.ToArray();
                        }
                    }
                    else
                    {

                        //string path = AppDomain.CurrentDomain.BaseDirectory + "defaultBook.jpg";
                        // string path = @"..\wwwroot\images\Temp\defaultBook.jpg";
                        string path = Environment.CurrentDirectory + @"/wwwroot/images/Temp/defaultBook.jpg";
                        byte[] image = System.IO.File.ReadAllBytes(path);

                        product.ProductImage = image;

                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");
            }

            PopulateCategoryDropDown(product.CategoryId);
            PopulateSupplierDropDown(product.SupplierId);
            return View(product);
        }

        // GET: AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            PopulateCategoryDropDown(product.CategoryId);
            PopulateSupplierDropDown(product.SupplierId);
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,SupplierId,ProductName,ProductPrice,ProductImage, Description")] Product product, IFormFile uploadFile)
        {
            /*
            var relativeName = "";
            var fileName = "";

            if (id != product.Id)
            {
                return NotFound();
            }

            if (uploadFile != null && uploadFile.Length > 0)
            {
                fileName = ContentDispositionHeaderValue.Parse(uploadFile.ContentDisposition).FileName.Trim('"');
                relativeName = "/ProductImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;
                using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                {
                    await uploadFile.CopyToAsync(fs);
                    fs.Flush();
                }
            } else {
                relativeName = product.ProductImage;
            }

            product.ProductImage = relativeName;
            */
           
           
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await uploadFile.CopyToAsync(memoryStream);
                            product.ProductImage = memoryStream.ToArray();
                        }
                    }
                    
                    
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateCategoryDropDown(product.CategoryId);
            PopulateSupplierDropDown(product.SupplierId);
            return View(product);
        }

        // GET: AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(c => c.Category)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<ActionResult> GetProductImage(int id)
        {

            var product = await _context.Products.SingleAsync(m => m.Id == id);

                
                if (product == null)
            {
                throw new ApplicationException($"Unable to product image with ID"+id+".");
            }
            byte[] bytes = product.ProductImage; //Get the image from your database
            return File(bytes, "image/jpeg"); //or "image/jpeg", depending on the format
            
        }
    }
}

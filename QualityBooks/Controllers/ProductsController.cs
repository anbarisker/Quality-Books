using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBooks.Data;
using QualityBooks.Models;
using Microsoft.AspNetCore.Authorization;
using QualityBooks.Controllers;
using Microsoft.AspNetCore.Hosting;

namespace QualityBooks.Controllers
{

    //[Authorize(Roles = "Member")]
    public class ProductsController : Controller
    {
        private readonly QualityBooksContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        public ProductsController(QualityBooksContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string SearchString, string currentFilter, int? id, int? page)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            ViewData["CurrentFilter"] = SearchString;

            var products = from p in _context.Products.Include(p => p.Category).Include(p => p.Supplier) select p;

            if (id != null)
            {
                products = products.Where(b => b.CategoryId == id);
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(b => b.ProductName.Contains(SearchString));
            }

            int pageSize = 8;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        

    }
}

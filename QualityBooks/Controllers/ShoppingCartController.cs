using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityBooks.Data;
using QualityBooks.Models;
using Microsoft.AspNetCore.Authorization;

namespace QualityBooks.Controllers
{
    [Authorize(Roles = "Member")]
    public class ShoppingCartController : Controller
    {

        QualityBooksContext _context;

        public ShoppingCartController(QualityBooksContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedTutorial = _context.Products.Single(product => product.Id == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedTutorial, _context);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id, _context);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult ClearCart()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.ClearCart(_context);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

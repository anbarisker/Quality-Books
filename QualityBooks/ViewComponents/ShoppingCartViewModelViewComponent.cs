using QualityBooks.Data;
using QualityBooks.Models;
using QualityBooks.Models.ShoppingCartViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualitySouvenirs.ViewComponents
{
    public class ShoppingCartViewModelViewComponent : ViewComponent
    {
        private readonly QualityBooksContext _context;
        public ShoppingCartViewModelViewComponent(QualityBooksContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(ReturnCurrentCartViewModel());
        }

        public ShoppingCartViewModel ReturnCurrentCartViewModel()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(_context),
                CartTotal = cart.GetTotal(_context)
            };
            return viewModel;
        }
    }
}

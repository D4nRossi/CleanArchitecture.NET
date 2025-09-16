// HomeController.cs (.NET 5)
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchMvc.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Pegue poucos itens só para vitrine
            var products = ( await _productService.GetProductsAsync() )?.Take(8).ToList();
            var categories = ( await _categoryService.GetCategoriesAsync() )?.Take(8).ToList();

            ViewBag.FeaturedProducts = products;
            ViewBag.TopCategories = categories;

            return View();
        }

        public IActionResult Privacy() => View();
    }
}

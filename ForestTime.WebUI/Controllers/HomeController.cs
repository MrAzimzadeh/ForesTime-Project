using ForestTime.WebUI.Data;
using ForestTime.WebUI.Models;
using ForestTime.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ForestTime.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //The readonly keyword before the field declarations indicates that these fields can only be assigned a value within the constructor or at the time of declaration.
        //Once assigned, their values cannot be modified, ensuring that they remain constant within the HomeController instance.
        //This is a common practice for fields that are intended to be set once and used throughout the lifespan of the object.
        private readonly ILogger<HomeController> _logger;
        //By using a logger, developers can record and track events, errors,
        //and other information during the execution of the application.
        private readonly AppDbContext _context;
        //The database context represents a session with the database and is used to perform various database operations
        //such as querying, inserting, updating, and deleting data.
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            //By doing this, the logger and database context are made available throughout the class,
            //allowing the methods within HomeController to utilize them for logging and database operations.
            _logger = logger;
            _context = context;
           
        }

        public IActionResult Index()
        {
            var articles=_context.Articles.Include(x=>x.User).ToList();
            var tags=_context.Tags.ToList();
            var categories=_context.Categories.ToList();
            HomeVM vm = new()
            {
                HomeArticles = articles,
                HomeTags = tags,
                HomeCategories=categories
            };
            return View(vm);
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
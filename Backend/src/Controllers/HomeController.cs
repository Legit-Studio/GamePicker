using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
    }
}
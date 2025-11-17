namespace DarkBootstrap.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Page1()
    {
        return View("PageX");
    }

    public IActionResult Page2()
    {
        return View("PageX");
    }

    public IActionResult Page3()
    {
        return View("PageX");
    }

    public IActionResult Page4()
    {
        return View("PageX");
    }

    public IActionResult Page5()
    {
        return View("PageX");
    }
}
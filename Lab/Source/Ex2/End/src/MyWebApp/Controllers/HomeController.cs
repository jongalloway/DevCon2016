namespace MyWebApp.Controllers
{
    using Microsoft.AspNet.Mvc;

    public class HomeController : Controller
    {
        [HttpGet()]
        public string Index() => "Hello from MVC!";
    }
}

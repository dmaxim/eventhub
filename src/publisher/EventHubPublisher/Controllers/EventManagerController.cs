using EventHubPublisher.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventHubPublisher.Controllers
{
    public class EventManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
	        return View();
        }


        [HttpPost]
        public IActionResult Create(NewEventModel newEventModel)
        {
	        return RedirectToAction("Index");
        }
    }
}
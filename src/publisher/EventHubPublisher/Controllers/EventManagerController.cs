using System.Threading.Tasks;
using EventHubPublisher.Managers;
using EventHubPublisher.Models;
using Microsoft.AspNetCore.Mvc;
using Mx.EventHub.Model;

namespace EventHubPublisher.Controllers
{
    public class EventManagerController : Controller
    {
	    private readonly IEventHubManager _eventHubManager;

	    public EventManagerController(IEventHubManager eventHubManager)
	    {
		    _eventHubManager = eventHubManager;
	    }
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
	        return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventTypeOne()
        {
	        await _eventHubManager.SendEventAsync(new EventMessageModel("Event Type One Created"))
		        .ConfigureAwait(false);
			
	        return RedirectToAction("Create");
		}

        [HttpPost]
        public async Task<IActionResult> CreateEventTypeTwo()
        {
	        await _eventHubManager.SendEventAsync(new EventMessageModel("Event Type Two Created"))
		        .ConfigureAwait(false);

			return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventBatch()
		{

			var eventBatch = new EventMessageModel[]
			{
				new EventMessageModel("Batch Event One Created"),
				new EventMessageModel("Batch Event Two Created"),
				new EventMessageModel("Batch Event Three Created")
				,new EventMessageModel("Batch Event Four Created")
			};
	        await _eventHubManager.SendEventsAsync(eventBatch)
		        .ConfigureAwait(false);

	        return RedirectToAction("Create");
		}
	}
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Event_Ease.Data;
using Event_Ease.Models;

namespace Event_Ease.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventsController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = _context.Events
    .Include(e => e.Venue)
    .Include(e => e.EventType);
            return View(await events.ToListAsync());
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(
                _context.Venues,
                "VenueId",
                "VenueName");

            ViewData["EventTypeId"] = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "TypeName");

            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Event events,
            IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // IMAGE UPLOAD
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images");

                    string uniqueFileName =
                        Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    events.ImageUrl = "/images/" + uniqueFileName;
                }

                _context.Add(events);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueId"] = new SelectList(
     _context.Venues,
     "VenueId",
     "VenueName",
     events.VenueId);

            ViewData["EventTypeId"] = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "TypeName",
                events.EventTypeId);

            return View(events);
        }
        // SHOW EVENTS BY VENUE
        public async Task<IActionResult> ByVenue(int venueId)
        {
            var events = await _context.Events
                .Include(e => e.Venue)
                .Where(e => e.VenueId == venueId)
                .ToListAsync();

            return View(events);
        }
    }
}
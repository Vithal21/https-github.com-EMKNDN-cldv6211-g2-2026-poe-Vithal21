using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Event_Ease.Data;
using Event_Ease.Models;

namespace Event_Ease.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(
     int? eventTypeId,
     DateTime? startDate,
     DateTime? endDate)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .ThenInclude(e => e.EventType)
                .Include(b => b.Event)
                .ThenInclude(e => e.Venue)
                .AsQueryable();

            if (eventTypeId.HasValue)
            {
                bookings = bookings.Where(b =>
                    b.Event != null &&
                    b.Event.EventTypeId == eventTypeId);
            }

            if (startDate.HasValue)
            {
                bookings = bookings.Where(b =>
                    b.Event != null &&
                    b.Event.StartDate >= startDate);
            }

            if (endDate.HasValue)
            {
                bookings = bookings.Where(b =>
                    b.Event != null &&
                    b.Event.EndDate <= endDate);
            }

            ViewBag.EventTypes = new SelectList(
                _context.EventTypes,
                "EventTypeId",
                "TypeName");

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            return View(booking);
        }
    }
}
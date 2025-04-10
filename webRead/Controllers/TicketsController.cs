using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;

namespace webRead.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return View(tickets);
        }

        // POST: Tickets/EditStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, [FromForm] int status)
        {
            // Log incoming values for debugging
            Console.WriteLine($"Received EditStatus request: ID={id}, Status={status}");

            if (status < 0 || status > 2)
            {
                TempData["SwalError"] = "Invalid status value.";
                return RedirectToAction(nameof(Index));
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                TempData["SwalError"] = $"Ticket with ID {id} not found.";
                return NotFound();
            }

            // Log current and new status
            Console.WriteLine($"Current Status: {ticket.Status}, New Status: {status}");

            if (ticket.Status == status)
            {
                TempData["SwalSuccess"] = "Status unchanged (already set to this value).";
                return RedirectToAction(nameof(Index));
            }

            ticket.Status = status;
            try
            {
                _context.Update(ticket);
                var rowsAffected = await _context.SaveChangesAsync();
                Console.WriteLine($"Rows affected: {rowsAffected}");
                TempData["SwalSuccess"] = $"Ticket status updated to {GetStatusName(status)} successfully!";
            }
            catch (DbUpdateException ex)
            {
                TempData["SwalError"] = "Failed to update ticket status in the database.";
                Console.WriteLine($"DbUpdateException: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An unexpected error occurred.";
                Console.WriteLine($"Exception: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        private string GetStatusName(int status)
        {
            return status switch
            {
                0 => "UnPaid",
                1 => "Paid",
                2 => "Rejected",
                _ => "Unknown"
            };
        }
    }
}
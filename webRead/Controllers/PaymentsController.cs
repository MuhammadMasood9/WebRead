using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;

namespace webRead.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments.ToListAsync();
            return View(payments);
        }

        // POST: Payments/EditStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, [FromForm] int status)
        {
            // Validate status range
            if (status < 0 || status > 2)
            {
                TempData["SwalError"] = "Invalid status value.";
                return RedirectToAction(nameof(Index));
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                TempData["SwalError"] = $"Payment with ID {id} not found.";
                return NotFound();
            }

            // Skip update if status is unchanged
            if (payment.Status == status)
            {
                TempData["SwalSuccess"] = "Status unchanged (already set to this value).";
                return RedirectToAction(nameof(Index));
            }

            payment.Status = status;
            try
            {
                _context.Update(payment);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = $"Payment status updated to {GetStatusName(status)} successfully!";
            }
            catch (DbUpdateException ex)
            {
                TempData["SwalError"] = "Failed to update payment status in the database.";
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using TaskAlias = System.Threading.Tasks.Task;

namespace webRead.Controllers
{
    public class DonationDropdownsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationDropdownsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DonationDropdowns
        public async Task<IActionResult> Index()
        {
            var donationDropdowns = await _context.DonationDropdowns
                .ToListAsync();
            return View(donationDropdowns);
        }

        // GET: DonationDropdowns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonationDropdowns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonationType,DonationStatus")] DonationDropdown donationDropdown)
        {
            if (ModelState.IsValid)
            {
                donationDropdown.CreatedAt = DateTime.UtcNow;
                _context.Add(donationDropdown);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Donation Dropdown created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(donationDropdown);
        }

        // GET: DonationDropdowns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donationDropdown = await _context.DonationDropdowns.FindAsync(id);
            if (donationDropdown == null)
            {
                return NotFound();
            }
            return View(donationDropdown);
        }

        // POST: DonationDropdowns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DonationType,DonationStatus,CreatedAt")] DonationDropdown donationDropdown)
        {
            if (id != donationDropdown.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donationDropdown);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Donation Dropdown updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationDropdownExists(donationDropdown.ID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donationDropdown);
        }

        // POST: DonationDropdowns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donationDropdown = await _context.DonationDropdowns.FindAsync(id);
            if (donationDropdown != null)
            {
                _context.DonationDropdowns.Remove(donationDropdown);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Donation Dropdown deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DonationDropdownExists(int id)
        {
            return _context.DonationDropdowns.Any(e => e.ID == id);
        }
    }
}
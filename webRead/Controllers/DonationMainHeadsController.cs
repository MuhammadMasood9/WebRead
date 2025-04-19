using Microsoft.AspNetCore.Mvc;
using webRead.Data;
using webRead.Models;
using Microsoft.EntityFrameworkCore;


namespace webRead.Controllers
{
    public class DonationMainHeadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationMainHeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DonationMainHeads
        public async Task<IActionResult> Index()
        {
            var donationMainHeads = await _context.DonationMainHeads
                .Take(1000)
                .ToListAsync();
            return View(donationMainHeads);
        }

        // GET: DonationMainHeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donationMainHead = await _context.DonationMainHeads
                .FirstOrDefaultAsync(m => m.MID == id);
            if (donationMainHead == null)
            {
                return NotFound();
            }

            return View(donationMainHead);
        }

        // GET: DonationMainHeads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonationMainHeads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Image,Status")] DonationMainHead donationMainHead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donationMainHead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donationMainHead);
        }

        // GET: DonationMainHeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donationMainHead = await _context.DonationMainHeads.FindAsync(id);
            if (donationMainHead == null)
            {
                return NotFound();
            }
            return View(donationMainHead);
        }

        // POST: DonationMainHeads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MID,Title,Image,Status")] DonationMainHead donationMainHead)
        {
            if (id != donationMainHead.MID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donationMainHead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationMainHeadExists(donationMainHead.MID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donationMainHead);
        }

        // GET: DonationMainHeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donationMainHead = await _context.DonationMainHeads
                .FirstOrDefaultAsync(m => m.MID == id);
            if (donationMainHead == null)
            {
                return NotFound();
            }

            return View(donationMainHead);
        }

        // POST: DonationMainHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donationMainHead = await _context.DonationMainHeads.FindAsync(id);
            if (donationMainHead != null)
            {
                _context.DonationMainHeads.Remove(donationMainHead);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DonationMainHeadExists(int id)
        {
            return _context.DonationMainHeads.Any(e => e.MID == id);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using TaskAlias = System.Threading.Tasks.Task;

namespace webRead.Controllers
{
    public class TaxCertificatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaxCertificatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaxCertificates
        public async Task<IActionResult> Index()
        {
            var taxCertificates = await _context.TaxCertificates
                .ToListAsync();
            return View(taxCertificates);
        }

        // GET: TaxCertificates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaxCertificates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,IssuedDate,Status,Message")] TaxCertificate taxCertificate, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        string base64String = Convert.ToBase64String(memoryStream.ToArray());
                        taxCertificate.ImageUrl = base64String;
                    }
                }
                taxCertificate.CreatedAt = DateTime.UtcNow;
                _context.Add(taxCertificate);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Tax Certificate created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(taxCertificate);
        }

        // GET: TaxCertificates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxCertificate = await _context.TaxCertificates.FindAsync(id);
            if (taxCertificate == null)
            {
                return NotFound();
            }
            return View(taxCertificate);
        }

        // POST: TaxCertificates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IssuedDate,CreatedAt,Status,Message")] TaxCertificate taxCertificate, IFormFile imageFile)
        {
            if (id != taxCertificate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCert = await _context.TaxCertificates.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                    if (existingCert == null)
                    {
                        return NotFound();
                    }

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            taxCertificate.ImageUrl = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                    else
                    {
                        taxCertificate.ImageUrl = existingCert.ImageUrl; // Retain existing image
                    }

                    _context.Update(taxCertificate);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Tax Certificate updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxCertificateExists(taxCertificate.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taxCertificate);
        }

        // POST: TaxCertificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxCertificate = await _context.TaxCertificates.FindAsync(id);
            if (taxCertificate != null)
            {
                _context.TaxCertificates.Remove(taxCertificate);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Tax Certificate deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TaxCertificateExists(int id)
        {
            return _context.TaxCertificates.Any(e => e.Id == id);
        }
    }
}
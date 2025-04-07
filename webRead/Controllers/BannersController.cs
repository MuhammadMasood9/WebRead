using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;
using System.Threading.Tasks;
using webRead.Models.webRead.Models;

namespace webRead.Controllers
{
    public class BannersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BannersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display the list of banners
        public async Task<IActionResult> Index()
        {
            var banners = await _context.Banners.ToListAsync();
            return View(banners);
        }

        // Display the Create form
        public IActionResult Create()
        {
            var banner = new Banner
            {
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            return View(banner);
        }

        // Handle the Create form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
            // Ensure CreatedAt is properly set
            if (banner.CreatedAt == default)
            {
                banner.CreatedAt = DateTime.UtcNow;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(banner);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Banner created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(banner);
        }

        // Display the Edit form
        public async Task<IActionResult> Edit(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                TempData["SwalError"] = "Banner not found!";
                return NotFound();
            }
            return View(banner);
        }

        // Handle the Edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BannerId,AdminId,Title,Description,ImageUrl,BannerLink,IsActive,CreatedAt")] Banner banner)
        {
            if (id != banner.BannerId)
            {
                TempData["SwalError"] = "Invalid banner ID!";
                return NotFound();
            }

            // Set the UpdatedAt timestamp
            banner.UpdatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Banner updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Banners.Any(e => e.BannerId == id))
                    {
                        TempData["SwalError"] = "Banner not found!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.InnerException?.Message ?? ex.Message);
                    return View(banner);
                }
            }
            return View(banner);
        }

        // Display the Delete confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                TempData["SwalError"] = "Banner not found!";
                return NotFound();
            }
            return View(banner);
        }

        // Handle the Delete submission
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Banner deleted successfully!";
            }
            else
            {
                TempData["SwalError"] = "Banner not found!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Toggle banner activation status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                TempData["SwalError"] = "Banner not found!";
                return NotFound();
            }

            try
            {
                banner.IsActive = !banner.IsActive;
                banner.UpdatedAt = DateTime.UtcNow;
                _context.Update(banner);
                await _context.SaveChangesAsync();

                TempData["SwalSuccess"] = banner.IsActive
                    ? "Banner activated successfully!"
                    : "Banner deactivated successfully!";
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
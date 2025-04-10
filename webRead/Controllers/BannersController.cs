using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;
using System.Threading.Tasks;

namespace webRead.Controllers
{
    public class BannersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BannersController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _context.Banners.ToListAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            var banner = new Banner
            {
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
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
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }
            return View(banner);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BannerId,AdminId,Title,Description,ImageUrl,BannerLink,IsActive,CreatedAt")] Banner banner)
        {
            if (id != banner.BannerId)
            {
                TempData["SwalError"] = "Invalid banner ID!";
                return NotFound();
            }

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
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }
            return View(banner);
        }

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
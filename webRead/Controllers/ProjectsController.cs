using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;
using System.Threading.Tasks;

namespace webRead.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        public IActionResult Create()
        {
            var project = new Project
            {
                CreatedAt = DateTime.UtcNow,
                Status = 1 // Default to active
            };
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (project.CreatedAt == default)
            {
                project.CreatedAt = DateTime.UtcNow;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Project created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                TempData["SwalError"] = "Project not found!";
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,Amount,Image,CreatedAt,Status,ProjectDescription")] Project project)
        {
            if (id != project.ProjectId)
            {
                TempData["SwalError"] = "Invalid project ID!";
                return NotFound();
            }

            project.UpdatedAt = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Project updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Projects.Any(p => p.ProjectId == id))
                    {
                        TempData["SwalError"] = "Project not found!";
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
            return View(project);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                TempData["SwalError"] = "Project not found!";
                return NotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Project deleted successfully!";
            }
            else
            {
                TempData["SwalError"] = "Project not found!";
            }
            return RedirectToAction(nameof(Index));
        }

        // New action to toggle status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                TempData["SwalError"] = "Project not found!";
                return NotFound();
            }

            try
            {
                project.Status = project.Status == 1 ? 0 : 1; // Toggle between 0 and 1
                project.UpdatedAt = DateTime.UtcNow;
                _context.Update(project);
                await _context.SaveChangesAsync();

                TempData["SwalSuccess"] = project.Status == 1
                    ? "Project activated successfully!"
                    : "Project deactivated successfully!";
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;
using System.Diagnostics;

namespace webRead.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project) // Include related Project data
                .ToListAsync();
            return View(tasks);
        }

        public IActionResult Create()
        {
            var task = new TaskProject
            {
                CreatedAt = DateTime.UtcNow,
                Status = 1 // Default to active
            };
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Create([Bind("ProjectId,Name,Progress,Report,Status,CreatedAt")] TaskProject task)
        {
            if (task.CreatedAt == null)
            {
                task.CreatedAt = DateTime.UtcNow;
            }

            // Log incoming values for debugging
            Debug.WriteLine($"MyError: ProjectId: {task.ProjectId}, Name: {task.Name}, Progress: {task.Progress}, Status: {task.Status}, Report: {(task.Report != null ? "Set" : "Null")}");

            // Remove Project from ModelState to prevent navigation property validation
            if (ModelState.ContainsKey("Project"))
            {
                ModelState.Remove("Project");
            }
            foreach (var key in ModelState.Keys)
            {
                Debug.WriteLine($"Key: {key}, Value: {ModelState[key].RawValue}, Errors: {string.Join(", ", ModelState[key].Errors.Select(e => e.ErrorMessage))}");
            }
            // Log ModelState keys before validation
            Debug.WriteLine("ModelState Keys Before Validation: " + string.Join(", ", ModelState.Keys));

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToList();
                TempData["ValidationErrors"] = string.Join("; ", errors.Select(e => $"{e.Key}: {string.Join(", ", e.Errors.Select(err => err.ErrorMessage))}"));
                Debug.WriteLine("Validation Errors: " + TempData["ValidationErrors"]);
                return View(task);
            }

            try
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Task created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                Debug.WriteLine($"Exception: {ex.Message}, Inner: {ex.InnerException?.Message}");
            }
            return View(task);
        }
        public async System.Threading.Tasks.Task<IActionResult> Edit(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                TempData["SwalError"] = "Task not found!";
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Edit(int id, [Bind("TaskId,ProjectId,Name,Progress,Report,Status,CreatedAt")] TaskProject task)
        {
            if (id != task.TaskId)
            {
                TempData["SwalError"] = "Invalid task ID!";
                return NotFound();
            }

            // Log incoming values for debugging
            Debug.WriteLine($"Edit MyError: TaskId: {task.TaskId}, ProjectId: {task.ProjectId}, Name: {task.Name}, Progress: {task.Progress}, Status: {task.Status}, Report: {(task.Report != null ? "Set" : "Null")}, CreatedAt: {task.CreatedAt}");

            // Remove Project from ModelState to prevent navigation property validation
            if (ModelState.ContainsKey("Project"))
            {
                ModelState.Remove("Project");
            }

            // Log ModelState keys before validation
            Debug.WriteLine("ModelState Keys Before Validation: " + string.Join(", ", ModelState.Keys));

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToList();
                TempData["ValidationErrors"] = string.Join("; ", errors.Select(e => $"{e.Key}: {string.Join(", ", e.Errors.Select(err => err.ErrorMessage))}"));
                Debug.WriteLine("Validation Errors: " + TempData["ValidationErrors"]);
                return View(task);
            }

            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Task updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tasks.Any(t => t.TaskId == id))
                {
                    TempData["SwalError"] = "Task not found!";
                    return NotFound();
                }
                else
                    throw;
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                Debug.WriteLine($"Exception: {ex.Message}, Inner: {ex.InnerException?.Message}");
            }
            return View(task);
        }
        public async System.Threading.Tasks.Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                TempData["SwalError"] = "Task not found!";
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Task deleted successfully!";
            }
            else
            {
                TempData["SwalError"] = "Task not found!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> ToggleStatus(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                TempData["SwalError"] = "Task not found!";
                return NotFound();
            }

            try
            {
                task.Status = task.Status == 1 ? 0 : 1;
                _context.Update(task);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = task.Status == 1
                    ? "Task activated successfully!"
                    : "Task deactivated successfully!";
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
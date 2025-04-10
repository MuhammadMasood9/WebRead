using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace webRead.Controllers
{
    public class ProjectUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectUsersController(ApplicationDbContext context)
        {
            _context = context;
            Console.WriteLine($"Connected to database: {_context.Database.GetDbConnection().Database}");
        }

        // GET: ProjectUsers
        public async Task<IActionResult> Index()
        {
            try
            {
                var projectUsers = await _context.ProjectUsers
                    .Include(pu => pu.Project)
                    .Include(pu => pu.Task)
                    .ToListAsync();
                return View(projectUsers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                Debug.WriteLine($"Error in Index: {ex.Message}");
                TempData["SwalError"] = "An error occurred while fetching assignments: " + ex.Message;
                return View(new List<ProjectUser>());
            }
        }

        // GET: ProjectUsers/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.Projects = _context.Projects.ToList();
                ViewBag.Tasks = new List<TaskProject>();
                ViewBag.Users = _context.Donors
                    .Select(d => new Donor
                    {
                        CorrespondenceId = d.CorrespondenceId,
                        Name = d.Name ?? "Unknown",
                        Email = d.Email ?? "",
                        Mobile = d.Mobile ?? ""
                    })
                    .ToList();
                return View(new ProjectUser());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create GET: {ex.Message}");
                Debug.WriteLine($"Error in Create GET: {ex.Message}");
                TempData["SwalError"] = "An error occurred while loading the form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProjectUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectID,UserId,TaskId")] ProjectUser projectUser)
        {
            Debug.WriteLine($"Create MyError: ProjectID: {projectUser.ProjectID}, UserId: {projectUser.UserId}, TaskId: {projectUser.TaskId}");

            // Clear any inherited ModelState errors to focus on our custom validation
            ModelState.Clear();

            // Validate ProjectID
            if (projectUser.ProjectID == 0 || !_context.Projects.Any(p => p.ProjectId == projectUser.ProjectID))
            {
                ModelState.AddModelError("ProjectID", "Please select a valid project.");
            }

            // Validate UserId
            if (projectUser.UserId == 0 || !_context.Donors.Any(d => d.CorrespondenceId == projectUser.UserId))
            {
                ModelState.AddModelError("UserId", "Please select a valid user.");
            }

            // Validate TaskId (optional, but must belong to ProjectID if provided)
            if (projectUser.TaskId.HasValue && projectUser.TaskId != 0)
            {
                var taskExists = await _context.Tasks
                    .AnyAsync(t => t.TaskId == projectUser.TaskId.Value && t.ProjectId == projectUser.ProjectID);
                if (!taskExists)
                {
                    ModelState.AddModelError("TaskId", $"Task {projectUser.TaskId} does not exist or does not belong to Project {projectUser.ProjectID}.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(projectUser);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Assignment created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError("", "Failed to save the assignment: " + ex.Message);
                }
            }

            // Log and display detailed validation errors
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");
            TempData["ValidationErrors"] = string.Join("; ", errors);
            Debug.WriteLine("Validation Errors: " + TempData["ValidationErrors"]);

            // Repopulate ViewBag
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Tasks = _context.Tasks.Where(t => t.ProjectId == projectUser.ProjectID).ToList();
            ViewBag.Users = _context.Donors.ToList();
            return View(projectUser);
        }

        // GET: ProjectUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["SwalError"] = "Assignment ID is required!";
                return NotFound();
            }

            try
            {
                var projectUser = await _context.ProjectUsers
                    .FirstOrDefaultAsync(pu => pu.ProjectUserId == id);

                if (projectUser == null)
                {
                    TempData["SwalError"] = "Assignment not found!";
                    return NotFound();
                }

                ViewBag.Projects = _context.Projects.ToList();
                ViewBag.Tasks = _context.Tasks.Where(t => t.ProjectId == projectUser.ProjectID).ToList();
                ViewBag.Users = _context.Donors.ToList();
                return View(projectUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit GET: {ex.Message}");
                Debug.WriteLine($"Error in Edit GET: {ex.Message}");
                TempData["SwalError"] = "An error occurred while loading the form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Project,NoUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectUserId,ProjectID,UserId,TaskId")] ProjectUser projectUser)
        {
            if (id != projectUser.ProjectUserId)
            {
                TempData["SwalError"] = "Invalid assignment ID!";
                return NotFound();
            }

            Debug.WriteLine($"Edit MyError: ProjectUserId: {projectUser.ProjectUserId}, ProjectID: {projectUser.ProjectID}, UserId: {projectUser.UserId}, TaskId: {projectUser.TaskId}");

            // Clear ModelState to avoid inherited errors
            ModelState.Clear();

            // Validate ProjectID
            if (projectUser.ProjectID == 0 || !_context.Projects.Any(p => p.ProjectId == projectUser.ProjectID))
            {
                ModelState.AddModelError("ProjectID", "Please select a valid project.");
            }

            // Validate UserId
            if (projectUser.UserId == 0 || !_context.Donors.Any(d => d.CorrespondenceId == projectUser.UserId))
            {
                ModelState.AddModelError("UserId", "Please select a valid user.");
            }

            // Validate TaskId (optional)
            if (projectUser.TaskId.HasValue && projectUser.TaskId != 0)
            {
                var taskExists = await _context.Tasks
                    .AnyAsync(t => t.TaskId == projectUser.TaskId.Value && t.ProjectId == projectUser.ProjectID);
                if (!taskExists)
                {
                    ModelState.AddModelError("TaskId", $"Task {projectUser.TaskId} does not exist or does not belong to Project {projectUser.ProjectID}.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectUser);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Assignment updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ProjectUsers.Any(pu => pu.ProjectUserId == projectUser.ProjectUserId))
                    {
                        TempData["SwalError"] = "Assignment not found!";
                        return NotFound();
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError("", "Failed to save the assignment: " + ex.Message);
                }
            }

            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");
            TempData["ValidationErrors"] = string.Join("; ", errors);
            Debug.WriteLine("Validation Errors: " + TempData["ValidationErrors"]);

            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Tasks = _context.Tasks.Where(t => t.ProjectId == projectUser.ProjectID).ToList();
            ViewBag.Users = _context.Donors.ToList();
            return View(projectUser);
        }

        // GET: ProjectUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["SwalError"] = "Assignment ID is required!";
                return NotFound();
            }

            try
            {
                var projectUser = await _context.ProjectUsers
                    .Include(pu => pu.Project)
                    .Include(pu => pu.Task)
                    .FirstOrDefaultAsync(pu => pu.ProjectUserId == id);

                if (projectUser == null)
                {
                    TempData["SwalError"] = "Assignment not found!";
                    return NotFound();
                }

                ViewBag.Users = _context.Donors.ToList();
                return View(projectUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete GET: {ex.Message}");
                Debug.WriteLine($"Error in Delete GET: {ex.Message}");
                TempData["SwalError"] = "An error occurred while loading the delete form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProjectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var projectUser = await _context.ProjectUsers.FindAsync(id);
                if (projectUser == null)
                {
                    TempData["SwalError"] = "Assignment not found!";
                    return RedirectToAction(nameof(Index));
                }

                _context.ProjectUsers.Remove(projectUser);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Assignment deleted successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete POST: {ex.Message}");
                Debug.WriteLine($"Error in Delete POST: {ex.Message}");
                TempData["SwalError"] = "An error occurred while deleting: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
        // AJAX endpoint to get tasks by ProjectId
        [HttpGet]
        public JsonResult GetTasksByProject(int projectId)
        {
            try
            {
                var tasks = _context.Tasks
                    .Where(t => t.ProjectId == projectId)
                    .Select(t => new { t.TaskId, t.Name })
                    .ToList();
                return Json(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTasksByProject: {ex.Message}");
                Debug.WriteLine($"Error in GetTasksByProject: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }
    }
}
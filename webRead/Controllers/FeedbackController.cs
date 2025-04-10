using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;
using System.Threading.Tasks;

namespace webRead.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index: List all feedback entries
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();
            return View(feedbacks);
        }

        // Create: Display form
        public IActionResult Create()
        {
            var feedback = new Feedback
            {
                SubmitedAt = DateTime.UtcNow
            };
            return View(feedback);
        }

        // Create: Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (feedback.SubmitedAt == default)
            {
                feedback.SubmitedAt = DateTime.UtcNow;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(feedback);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Feedback created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }
            return View(feedback);
        }

        // Edit: Display form
        public async Task<IActionResult> Edit(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                TempData["SwalError"] = "Feedback not found!";
                return NotFound();
            }
            return View(feedback);
        }

        // Edit: Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,CrospondingId,Name,Email,Message,Rating,SubmitedAt")] Feedback feedback)
        {
            if (id != feedback.FeedbackId)
            {
                TempData["SwalError"] = "Invalid feedback ID!";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                    TempData["SwalSuccess"] = "Feedback updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Feedbacks.Any(f => f.FeedbackId == id))
                    {
                        TempData["SwalError"] = "Feedback not found!";
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
            return View(feedback);
        }

        // Delete: Display confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                TempData["SwalError"] = "Feedback not found!";
                return NotFound();
            }
            return View(feedback);
        }

        // Delete: Handle confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Feedback deleted successfully!";
            }
            else
            {
                TempData["SwalError"] = "Feedback not found!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
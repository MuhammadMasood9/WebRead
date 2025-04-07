using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webRead.Data;
using webRead.Models;
using System;

public class PushNotificationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PushNotificationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Display the list of notifications
    public async Task<IActionResult> Index()
    {
        var notifications = await _context.PushNotifications.ToListAsync();
        return View(notifications);
    }

    // Display the Create form
    public IActionResult Create()
    {
        var notification = new PushNotification
        {
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        return View(notification);
    }

    // Handle the Create form submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PushNotification notification)
    {
        // Ensure CreatedAt is properly set
        if (notification.CreatedAt == default)
        {
            notification.CreatedAt = DateTime.UtcNow;
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Notification created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                ModelState.AddModelError("", "An error occurred while saving: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        return View(notification);
    }

    // Display the Edit form (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var notification = await _context.PushNotifications.FindAsync(id);
        if (notification == null)
        {
            TempData["SwalError"] = "Notification not found!";
            return NotFound();
        }
        return View(notification);
    }

    // Handle the Edit form submission (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NotificationId,Title,Message,TargetAudience,NotificationType,SentAt,IsSent,CreatedAt,IsRead")] PushNotification notification)
    {
        if (id != notification.NotificationId)
        {
            TempData["SwalError"] = "Invalid notification ID!";
            return NotFound();
        }

        // Handle potential date range issues
        if (notification.SentAt.HasValue)
        {
            // Ensure SentAt is within SQL Server datetime range (Jan 1, 1753 to Dec 31, 9999)
            if (notification.SentAt.Value.Year < 1753 || notification.SentAt.Value.Year > 9999)
            {
                ModelState.AddModelError("SentAt", "Date must be between January 1, 1753 and December 31, 9999.");
                return View(notification);
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(notification);
                await _context.SaveChangesAsync();
                TempData["SwalSuccess"] = "Notification updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PushNotifications.Any(e => e.NotificationId == id))
                {
                    TempData["SwalError"] = "Notification not found!";
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Detailed error message for debugging
                TempData["SwalError"] = "An error occurred: " + (ex.InnerException?.Message ?? ex.Message);
                ModelState.AddModelError("", "An error occurred while saving: " + ex.InnerException?.Message ?? ex.Message);
                return View(notification);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(notification);
    }

    // GET: Delete confirmation
    public async Task<IActionResult> Delete(int id)
    {
        var notification = await _context.PushNotifications.FindAsync(id);
        if (notification == null)
        {
            TempData["SwalError"] = "Notification not found!";
            return NotFound();
        }
        return View(notification);
    }

    // POST: Handle Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var notification = await _context.PushNotifications.FindAsync(id);
        if (notification != null)
        {
            _context.PushNotifications.Remove(notification);
            await _context.SaveChangesAsync();
            TempData["SwalSuccess"] = "Notification deleted successfully!";
        }
        else
        {
            TempData["SwalError"] = "Notification not found!";
        }
        return RedirectToAction(nameof(Index));
    }
}
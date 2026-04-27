using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResoCafe.Data;
using ResoCafe.Models;

namespace ResoCafe.Controllers;

[Authorize(Roles = "admin")]
public class AdminController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["Page"] = "admin";
        var bookings = await db.Bookings
            .Include(b => b.User)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return View(bookings);
    }

    // меню
    public async Task<IActionResult> Menu()
    {
        ViewData["Page"] = "admin";
        var items = await db.MenuItems.OrderBy(m => m.Category).ThenBy(m => m.Name).ToListAsync();
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddMenuItem(MenuItem item)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Menu));

        db.MenuItems.Add(item);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Menu));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var item = await db.MenuItems.FindAsync(id);
        if (item != null)
        {
            db.MenuItems.Remove(item);
            await db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Menu));
    }

    [HttpPost]
    public async Task<IActionResult> EditMenuItem(MenuItem model)
    {
        var item = await db.MenuItems.FindAsync(model.Id);
        if (item == null) return NotFound();

        item.Category = model.Category;
        item.Name = model.Name;
        item.Description = model.Description;
        item.Price = model.Price;
        item.IsAvailable = model.IsAvailable;

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Menu));
    }

    // бронь
    [HttpPost]
    public async Task<IActionResult> ConfirmBooking(int id)
    {
        var booking = await db.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = BookingStatus.Confirmed;
            await db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await db.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = BookingStatus.Cancelled;
            await db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}

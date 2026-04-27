using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResoCafe.Data;
using ResoCafe.Models;
using ResoCafe.ViewModels;
using System.Security.Claims;

namespace ResoCafe.Controllers;

public class HomeController(ApplicationDbContext db) : Controller
{
    public IActionResult Index()
    {
        ViewData["Page"] = "home";
        return View();
    }

    public async Task<IActionResult> Catalog()
    {
        ViewData["Page"] = "catalog";
        var items = await db.MenuItems
            .Where(m => m.IsAvailable)
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();

        return View(items);
    }

    public IActionResult Cart()
    {
        ViewData["Page"] = "cart";
        return View();
    }

    public IActionResult Checkout()
    {
        ViewData["Page"] = "checkout";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Booking(BookingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["BookingError"] = "Пожалуйста, заполните все обязательные поля";
            return RedirectToAction(nameof(Index));
        }

        var booking = new Booking
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Status = BookingStatus.Pending
        };

        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(idClaim, out var userId))
            booking.UserId = userId;

        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        TempData["BookingSuccess"] = "Заявка отправлена! Мы свяжемся с вами.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderViewModel model)
    {
        if (model.Items == null || model.Items.Count == 0)
            return BadRequest("Корзина пуста");

        var ids = model.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = await db.MenuItems
            .Where(m => ids.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id);

        var order = new Order
        {
            CustomerName = model.CustomerName,
            Phone = model.Phone,
            Address = model.Address,
            Status = OrderStatus.New
        };

        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(idClaim, out var userId))
            order.UserId = userId;

        foreach (var item in model.Items)
        {
            if (!menuItems.TryGetValue(item.MenuItemId, out var menuItem))
                continue;

            order.Items.Add(new OrderItem
            {
                MenuItemId = menuItem.Id,
                Quantity = item.Quantity,
                Price = menuItem.Price
            });
        }

        order.Total = order.Items.Sum(i => i.Price * i.Quantity);

        db.Orders.Add(order);
        await db.SaveChangesAsync();

        return Ok(new { orderId = order.Id });
    }
}

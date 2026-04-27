using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResoCafe.Data;
using ResoCafe.ViewModels;
using System.Security.Claims;

namespace ResoCafe.Controllers;

[Authorize]
public class ProfileController(ApplicationDbContext db, IWebHostEnvironment env) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await GetCurrentUser();
        if (user == null) return RedirectToAction("Auth", "Account");

        ViewData["Page"] = "profile";

        var orders = await db.Orders
            .Where(o => o.UserId == user.Id)
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        var bookings = await db.Bookings
            .Where(b => b.UserId == user.Id)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        var vm = new ProfileViewModel
        {
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            AvatarPath = user.AvatarPath,
            Orders = orders,
            Bookings = bookings
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateInfo(ProfileViewModel model)
    {
        var user = await GetCurrentUser();
        if (user == null) return RedirectToAction("Auth", "Account");

        if (!string.IsNullOrWhiteSpace(model.NewUsername) && model.NewUsername != user.Username)
        {
            if (await db.Users.AnyAsync(u => u.Username == model.NewUsername && u.Id != user.Id))
            {
                TempData["Error"] = "Такой логин уже занят";
                return RedirectToAction(nameof(Index));
            }
            user.Username = model.NewUsername;
        }

        if (!string.IsNullOrWhiteSpace(model.NewEmail) && model.NewEmail != user.Email)
        {
            if (await db.Users.AnyAsync(u => u.Email == model.NewEmail && u.Id != user.Id))
            {
                TempData["Error"] = "Эта почта уже используется";
                return RedirectToAction(nameof(Index));
            }
            user.Email = model.NewEmail;
        }

        if (!string.IsNullOrWhiteSpace(model.NewPhone))
            user.Phone = model.NewPhone;

        await db.SaveChangesAsync();
        TempData["Success"] = "Данные обновлены";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar)
    {
        var user = await GetCurrentUser();
        if (user == null) return RedirectToAction("Auth", "Account");

        if (avatar == null || avatar.Length == 0)
        {
            TempData["Error"] = "Выберите файл";
            return RedirectToAction(nameof(Index));
        }

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(avatar.FileName).ToLower();

        if (!allowed.Contains(ext))
        {
            TempData["Error"] = "Допустимые форматы: jpg, png, webp";
            return RedirectToAction(nameof(Index));
        }

        var avatarsDir = Path.Combine(env.WebRootPath, "assets", "img", "avatars");
        Directory.CreateDirectory(avatarsDir);

        var fileName = $"user_{user.Id}_{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(avatarsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await avatar.CopyToAsync(stream);

        if (!string.IsNullOrEmpty(user.AvatarPath))
        {
            var oldPath = Path.Combine(env.WebRootPath, user.AvatarPath.TrimStart('/'));
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
        }

        user.AvatarPath = $"/assets/img/avatars/{fileName}";
        await db.SaveChangesAsync();

        TempData["Success"] = "Аватарка обновлена";
        return RedirectToAction(nameof(Index));
    }

    private async Task<Models.User?> GetCurrentUser()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idClaim, out var id)) return null;
        return await db.Users.FindAsync(id);
    }
}

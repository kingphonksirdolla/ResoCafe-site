using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResoCafe.Data;
using ResoCafe.Models;
using ResoCafe.ViewModels;
using System.Security.Claims;

namespace ResoCafe.Controllers;

public class AccountController(ApplicationDbContext db) : Controller
{
    public IActionResult Auth(string? tab)
    {
        ViewData["Page"] = "auth";
        ViewData["Tab"] = tab ?? "login";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ViewData["Page"] = "auth";
        ViewData["Tab"] = "login";

        if (!ModelState.IsValid)
            return View("Auth", model);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
            return View("Auth", model);
        }

        await SignInUser(user);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewData["Page"] = "auth";
        ViewData["Tab"] = "register";

        if (!ModelState.IsValid)
            return View("Auth", model);

        if (await db.Users.AnyAsync(u => u.Username == model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "Такой логин уже занят");
            return View("Auth", model);
        }

        if (model.Email != null && await db.Users.AnyAsync(u => u.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Эта почта уже используется");
            return View("Auth", model);
        }

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            Phone = model.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Role = "user"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        await SignInUser(user);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
        );
    }
}

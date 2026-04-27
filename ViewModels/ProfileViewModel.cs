using System.ComponentModel.DataAnnotations;
using ResoCafe.Models;

namespace ResoCafe.ViewModels;

public class ProfileViewModel
{
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AvatarPath { get; set; }

    [EmailAddress(ErrorMessage = "Некорректный формат почты")]
    public string? NewEmail { get; set; }

    [Phone(ErrorMessage = "Некорректный номер телефона")]
    public string? NewPhone { get; set; }

    public string? NewUsername { get; set; }

    public List<Order> Orders { get; set; } = [];
    public List<Booking> Bookings { get; set; } = [];
}

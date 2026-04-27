namespace ResoCafe.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AvatarPath { get; set; }
    public string Role { get; set; } = "user";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}

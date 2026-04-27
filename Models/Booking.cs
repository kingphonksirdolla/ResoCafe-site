namespace ResoCafe.Models;

public class Booking
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled
}

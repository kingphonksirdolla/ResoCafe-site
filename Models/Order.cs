namespace ResoCafe.Models;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }

    public string CustomerName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Total { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = [];
}

public enum OrderStatus
{
    New,
    InProgress,
    Delivered,
    Cancelled
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

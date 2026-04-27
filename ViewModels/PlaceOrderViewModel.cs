using System.ComponentModel.DataAnnotations;

namespace ResoCafe.ViewModels;

public class PlaceOrderViewModel
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    public List<OrderItemViewModel> Items { get; set; } = [];
}

public class OrderItemViewModel
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}

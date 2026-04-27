using System.ComponentModel.DataAnnotations;

namespace ResoCafe.ViewModels;

public class BookingViewModel
{
    [Required(ErrorMessage = "Введите имя")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите фамилию")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите номер телефона")]
    public string Phone { get; set; } = string.Empty;
}

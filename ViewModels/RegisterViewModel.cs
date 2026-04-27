using System.ComponentModel.DataAnnotations;

namespace ResoCafe.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите логин")]
    [MinLength(3, ErrorMessage = "Минимум 3 символа")]
    public string Username { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Некорректный формат почты")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Некорректный номер телефона")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [MinLength(6, ErrorMessage = "Минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Повторите пароль")]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirm { get; set; } = string.Empty;
}

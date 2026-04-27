using System.ComponentModel.DataAnnotations;

namespace ResoCafe.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите логин")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace MyApp.Models.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models.AccountViewModels;

public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
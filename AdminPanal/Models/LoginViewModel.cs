using System.ComponentModel.DataAnnotations;

namespace AdminPanal.Models
{
    public class LoginViewModel
    {
            [EmailAddress]
            public string Email { get; set; }
            public string Password { get; set; }
    }
}

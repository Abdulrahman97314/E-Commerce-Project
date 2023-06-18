using System.ComponentModel.DataAnnotations;

namespace Store.APIs.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

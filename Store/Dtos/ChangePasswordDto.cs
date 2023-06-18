using System.ComponentModel.DataAnnotations;

namespace Store.APIs.Dtos
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        [MinLength(8)]
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set;}
    }
}

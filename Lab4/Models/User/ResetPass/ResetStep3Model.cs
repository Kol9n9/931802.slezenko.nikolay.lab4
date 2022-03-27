using System.ComponentModel.DataAnnotations;

namespace Lab4.Models.User.ResetPassword
{
    public class ResetStep3Model
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

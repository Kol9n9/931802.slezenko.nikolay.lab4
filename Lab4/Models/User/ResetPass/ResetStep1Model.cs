using System.ComponentModel.DataAnnotations;

namespace Lab4.Models.User.ResetPassword
{
    public class ResetStep1Model
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

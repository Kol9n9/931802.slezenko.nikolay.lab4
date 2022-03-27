using System.ComponentModel.DataAnnotations;

namespace Lab4.Models.User.ResetPassword
{
    public class ResetStep2Model
    {
        [Required]
        [RegularExpression(@"[0-9]{4}", ErrorMessage = "Incorrect code")]
        public string Code { get; set; }
    }
}

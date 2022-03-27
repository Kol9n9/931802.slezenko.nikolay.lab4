using System.ComponentModel.DataAnnotations;

namespace Lab4.Models.Controlls
{
    public class RadioModel
    {
        [Required]
        public MonthList Month { get; set; }
    }
}

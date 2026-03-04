using System.ComponentModel.DataAnnotations;

namespace M8ScoreTool.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ForestTime.WebUI.DTOs
{
    public class RegisterDTO
    {
        [MaxLength(15)]//data annotation
        [Required]//data annotation
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        //atribute emailin duzgunluyunu yoxlayir AOP
        [EmailAddress]//data annotation
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
    }
}

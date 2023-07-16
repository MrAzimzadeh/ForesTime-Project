using Microsoft.AspNetCore.Identity;

namespace ForestTime.WebUI.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
     

    }
}

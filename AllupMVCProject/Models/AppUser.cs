using Microsoft.AspNetCore.Identity;

namespace AllupMVCProject.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }    
    }
}

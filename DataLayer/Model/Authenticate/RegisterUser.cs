using Microsoft.AspNetCore.Identity;

namespace DataLayer.Model.Authenticate
{
    public class RegisterUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
        public string? Password { get; set; }
        public string? RepeatPassword { get; set; }
        public string? IntroducedCode { get; set; }
    }
}

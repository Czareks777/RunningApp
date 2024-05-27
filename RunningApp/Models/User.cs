using Microsoft.AspNetCore.Identity;

namespace RunningApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<RunningSession> RunningSession { get; set; }
    }
}

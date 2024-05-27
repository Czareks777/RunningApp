using RunningApp.Models;

namespace RunningApp.Repository.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user, TimeSpan expiration);
    }
}

using System.Diagnostics.CodeAnalysis;

namespace EcommerceElectronicsBackend.Models
{
    public class User
    {
    public int    UserId       { get; set; }
    public string Username     { get; set; } = null!;
    public string Email        { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role         { get; set; } = null!;
    }

}

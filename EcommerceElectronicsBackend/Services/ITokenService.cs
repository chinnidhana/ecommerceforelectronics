namespace EcommerceElectronicsBackend.Services
{
    using EcommerceElectronicsBackend.Models;

    public interface ITokenService
    {
        string CreateToken(User user);
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceElectronicsBackend.Data;
using EcommerceElectronicsBackend.Models;
using EcommerceElectronicsBackend.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace EcommerceElectronicsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenService _tokenService;

        // <-- Here is constructor injection
        public AuthController(ApplicationDbContext db, ITokenService tokenService)
        {
            _db           = db;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User creds)
        {
            if (await _db.Users.AnyAsync(u => u.Username == creds.Username))
                return BadRequest("Username already taken.");

            // generate salt + hash
            var salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: creds.PasswordHash,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8));

            var user = new User
            {
                Username     = creds.Username,
                PasswordHash = $"{Convert.ToBase64String(salt)}.{hash}",
                Role         = "Customer"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // <-- use injected service to create the JWT
            var token = _tokenService.CreateToken(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User creds)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == creds.Username);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            // split salt and hash
            var parts       = user.PasswordHash.Split('.');
            var salt        = Convert.FromBase64String(parts[0]);
            var storedHash  = parts[1];
            var incomingHash= Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: creds.PasswordHash,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8));

            if (incomingHash != storedHash)
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.CreateToken(user);
            return Ok(new { token });
        }
    }
}

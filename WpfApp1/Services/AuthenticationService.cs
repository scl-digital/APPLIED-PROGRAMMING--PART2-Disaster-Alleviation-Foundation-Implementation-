using System.Security.Cryptography;
using System.Text;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(LoginRequest request);
        Task<AuthenticationResult> RegisterAsync(RegisterRequest request);
        Task<bool> ValidateUserAsync(string email, string password);
        void Logout();
        User? GetCurrentUser();
        bool IsAuthenticated { get; }
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<User> _users = new();
        private User? _currentUser;
        
        public bool IsAuthenticated => _currentUser != null;
        
        public User? GetCurrentUser() => _currentUser;

        public async Task<AuthenticationResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = _users.FirstOrDefault(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));
                
                if (user == null)
                {
                    return AuthenticationResult.Failure("Invalid email or password");
                }
                
                if (!user.IsActive)
                {
                    return AuthenticationResult.Failure("Account is deactivated");
                }
                
                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                    return AuthenticationResult.Failure("Invalid email or password");
                }
                
                user.LastLoginAt = DateTime.UtcNow;
                _currentUser = user;
                
                return AuthenticationResult.Success(user);
            }
            catch (Exception ex)
            {
                return AuthenticationResult.Failure($"Login failed: {ex.Message}");
            }
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    return AuthenticationResult.Failure("Email already exists");
                }
                
                var user = new User
                {
                    Id = _users.Count + 1,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    Role = request.Role,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                
                _users.Add(user);
                _currentUser = user;
                
                return AuthenticationResult.Success(user);
            }
            catch (Exception ex)
            {
                return AuthenticationResult.Failure($"Registration failed: {ex.Message}");
            }
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return user != null && VerifyPassword(password, user.PasswordHash);
        }

        public void Logout()
        {
            _currentUser = null;
        }

        private string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[32];
            rng.GetBytes(salt);
            
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);
            
            var hashBytes = new byte[64];
            Array.Copy(salt, 0, hashBytes, 0, 32);
            Array.Copy(hash, 0, hashBytes, 32, 32);
            
            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                var hashBytes = Convert.FromBase64String(passwordHash);
                var salt = new byte[32];
                Array.Copy(hashBytes, 0, salt, 0, 32);
                
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
                var hash = pbkdf2.GetBytes(32);
                
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 32] != hash[i])
                        return false;
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

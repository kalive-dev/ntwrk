using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ntwrk.Api.Functions.User
{
    public class UserFunction : IUserFunction
    {
        private readonly ntwrkContext _ntwrkContext;

        public UserFunction(ntwrkContext ntwrkContext)
        {
            _ntwrkContext = ntwrkContext;
        }

        public User? Authenticate(string loginId, string password)
        {
            try
            {
                var entity = _ntwrkContext.TblUsers.SingleOrDefault(x => x.LoginId == loginId);
                if (entity == null) return null;

                var isPasswordMatched = VertifyPassword(password, entity.StoredSalt, entity.Password);

                if (!isPasswordMatched) return null;
                entity.LastLogonTime = DateTime.UtcNow;

                var token = GenerateJwtToken(entity);
                _ntwrkContext.SaveChangesAsync();
                return new User
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    Token = token
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<User>> Search(string SearchRequestData)
        {
            try
            {
                // Perform case-insensitive search (modify if needed)
                var searchText = SearchRequestData.Trim().ToLower();

                var entities = await _ntwrkContext.TblUsers
                  .Where(user => user.UserName.ToLower().Contains(searchText)
                                 || user.LoginId.ToLower().Contains(searchText)
                                // Add more search criteria based on your User model properties
                                )
                  .ToListAsync();

                return entities.Select(x => new User
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    AvatarSourceName = x.AvatarSourceName,
                    IsOnline = x.IsOnline,
                    IsAway = !x.IsOnline
                });
            }
            catch (Exception ex)
            {
                // Handle any exceptions during data access
                return null;
            }
        }

        public User? Register(string loginId, string userName, string password)
        {
            // 1. Validate user input (optional)
            if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null; // Or throw an exception for invalid input
            }

            // 2. Check for existing username
            if (_ntwrkContext.TblUsers.Any(u => u.LoginId == loginId))
            {
                return null; // Username already exists
            }

            // 3. Generate a secure password hash and salt
            byte[] salt = new byte[128 / 8];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
              password: password,
              salt: salt,
              prf: KeyDerivationPrf.HMACSHA1,
              iterationCount: 10000,
              numBytesRequested: 256 / 8));

            // 4. Create a new user entity
            var newUser = new TblUser
            {
                UserName = userName,
                LoginId = loginId,
                Password = hashedPassword,
                StoredSalt = salt,
                AvatarSourceName = "default_user.png",
                IsOnline = false,
                LastLogonTime = DateTime.Now,
            };

            // 5. Add the new user to the database context
            _ntwrkContext.TblUsers.Add(newUser);

            // 6. Save changes to the database
            try
            {
                _ntwrkContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle database saving exceptions
                return null;
            }
            var token = GenerateJwtToken(newUser);
            // 7. No need to return the newly created user with ID (auto-generated)
            // Return a success message or redirect to a confirmation page (optional)
            return new User
            {
                Id = newUser.Id,
                UserName = newUser.UserName,
                Token = token
            };
        }

        public User GetUserById(int id)
        {
            var entity = _ntwrkContext.TblUsers
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (entity == null) return new User();

            var awayDuration = entity.IsOnline ? "" : Utilities.CalcAwayDuration(entity.LastLogonTime);
            return new User
            {
                UserName = entity.UserName,
                Id = entity.Id,
                AvatarSourceName = entity.AvatarSourceName,
                IsAway = awayDuration != "" ? true : false,
                AwayDuration = awayDuration,
                IsOnline = entity.IsOnline,
                LastLogonTime = entity.LastLogonTime
            };
        }


        private bool VertifyPassword(string enteredPassword, byte[] storedSalt, string storedPassword)
        {
            string encryptyedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return encryptyedPassword.Equals(storedPassword);
        }

        private string GenerateJwtToken(TblUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}

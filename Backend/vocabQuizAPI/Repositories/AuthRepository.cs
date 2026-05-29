using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using vocabQuizAPI.Models;
using vocabQuizAPI.Models.Dtos;


namespace vocabQuizAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> RegisterAsync(RegisterDto request)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string checkQuery = @"SELECT COUNT(1) 
                                    FROM users 
                                    WHERE email = @Email"; // email var mi kontrol

                int exists = await db.ExecuteScalarAsync<int>(checkQuery, new { Email = request.Email }); // ilk rowu al executeScalar

                if (exists > 0) return null; // Email varsa

                // hash
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);


                string insertQuery = @"
                    INSERT INTO users (username, email, password_hash) 
                    VALUES (@Username, @Email, @PasswordHash);
                    SELECT LAST_INSERT_ID();";

                int newId = await db.ExecuteScalarAsync<int>(insertQuery, new
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash
                });

               
                return new User { UserId = newId, Username = request.Username, Email = request.Email };
            }
        }

        public async Task<User?> LoginAsync(LoginDto request)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                
                string query = @"SELECT 
                                user_id AS UserId,
                                username AS Username,
                                email AS Email,
                                password_hash AS PasswordHash, 
                                created_at AS CreatedAt
                                FROM users 
                                WHERE email = @Email";
                var user = await db.QueryFirstOrDefaultAsync<User>(query, new { Email = request.Email });

                if (user == null) return null; // kullanici yok

                // pass kontrol
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

                if (!isPasswordCorrect) return null; 

                return user;
            }
        }
    }
}


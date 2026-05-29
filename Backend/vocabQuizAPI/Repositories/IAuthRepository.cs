using vocabQuizAPI.Models;
using vocabQuizAPI.Models.Dtos;

namespace vocabQuizAPI.Repositories
{


    public interface IAuthRepository
    {
        Task<User?> RegisterAsync(RegisterDto request);
        Task<User?> LoginAsync(LoginDto request);
    }

}


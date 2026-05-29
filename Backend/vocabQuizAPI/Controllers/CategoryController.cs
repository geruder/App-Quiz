using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vocabQuizAPI.Repositories;

namespace vocabQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repo;

        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}/words")]
        public async Task<IActionResult> GetWords(int id)
        {
            var words = await _repo.GetWordsByCategoryAsync(id);
            return Ok(words);
        }
    }
}

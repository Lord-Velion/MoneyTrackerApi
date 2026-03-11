using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoneyTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(new { message = "Get all categories" });
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            return Ok(new { message = "Get category by id" });
        }

        [HttpPost]
        public IActionResult CreateCategory()
        {
            return Ok(new { message = "Create new category" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory()
        {
            return Ok(new { message = "Update category" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory()
        {
            return Ok(new { message = "Delete category" });
        }
    }
}

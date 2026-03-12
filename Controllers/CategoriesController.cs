using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTrackerApi.DTOs;
using MoneyTrackerApi.Models;
using System.Threading.Tasks;

namespace MoneyTrackerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MoneyTrackerDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriesController(MoneyTrackerDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }

        private async Task<bool> CategoryExistsForUser(int id, string userId)
        {
            return await _context.Categories.AnyAsync(e => e.Id == id && e.UserId == userId);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var userId = GetUserId();
            var categories = await _context.Categories
                .Where(c => c.UserId == userId) // Фильтр по пользователю
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return categories;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var userId = GetUserId();
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId); // Проверка принадлежности

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = GetUserId() // Устанавливаем пользователя
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO updateCategoryDTO)
        {
            var userId = GetUserId();
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId); // Проверка принадлежности

            if (category == null)
            {
                return NotFound();
            }

            category.Name = updateCategoryDTO.Name;
            category.Description = updateCategoryDTO.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExistsForUser(id, userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = GetUserId();
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId); // Проверка принадлежности

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

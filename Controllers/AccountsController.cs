using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTrackerApi.Models;
using System.Threading.Tasks;

namespace MoneyTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly MoneyTrackerDbContext _context;

        public AccountsController(MoneyTrackerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public IActionResult GetAccountById(int id)
        {
            return Ok(new { message = "Get account by id" });
        }

        [HttpPost]
        public IActionResult CreateAccount()
        {
            return Ok(new { message = "Create new account" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount()
        {
            return Ok(new { message = "Update account" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount()
        {
            return Ok(new { message = "Delete account" });
        }

    }
}

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
    public class AccountsController : ControllerBase
    {
        private readonly MoneyTrackerDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(MoneyTrackerDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }

        private async Task<bool> AccountExistsForUser(int id, string userId)
        {
            return await _context.Accounts.AnyAsync(e => e.Id == id && e.UserId == userId);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            var userId = GetUserId();
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => new AccountDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description
                })     
                .ToListAsync();

            return accounts;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {
            var userId = GetUserId();
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(CreateAccountDTO dto)
        {
            var account = new Account
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = GetUserId()
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, UpdateAccountDTO updateAccountDTO)
        {
            var userId = GetUserId();
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null)
            {
                return NotFound();
            }

            account.Name = updateAccountDTO.Name;
            account.Description = updateAccountDTO.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AccountExistsForUser(id, userId))
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
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = GetUserId();
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

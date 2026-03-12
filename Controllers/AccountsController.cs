using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTrackerApi.DTOs;
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

        private async Task<bool> AccountExists(int id)
        {
            return await _context.Accounts.AnyAsync(e => e.Id == id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            var accounts = await _context.Accounts
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
            var account = await _context.Accounts.FindAsync(id);

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
                Description = dto.Description
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, UpdateAccountDTO updateAccountDTO)
        { 

            var account = await _context.Accounts.FindAsync(id);

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
                if (!await AccountExists(id))
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
            var account = await _context.Accounts.FindAsync(id);
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

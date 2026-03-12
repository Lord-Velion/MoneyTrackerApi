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
    public class TransactionsController : ControllerBase
    {
        private readonly MoneyTrackerDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsController(MoneyTrackerDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }

        private async Task<bool> TransactionExistsForUser(int id, string userId)
        {
            return await _context.Transactions.AnyAsync(e => e.Id == id && e.UserId == userId);
        }

        private async Task<bool> AccountAndCategoryBelongToUser(int accountId, int categoryId, string userId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            var category = await _context.Categories.FindAsync(categoryId);
            return account != null && account.UserId == userId &&
                   category != null && category.UserId == userId;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {
            var userId = GetUserId();
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId) // Фильтр по пользователю
                .Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Description = t.Description,
                    Date = t.Date,
                    IsIncome = t.IsIncome,
                    AccountId = t.AccountId,
                    CategoryId = t.CategoryId
                })
                .ToListAsync();

            return transactions;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(int id)
        {
            var userId = GetUserId();
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId); // Проверка принадлежности

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(CreateTransactionDTO dto)
        {
            var userId = GetUserId();

            if (!await AccountAndCategoryBelongToUser(dto.AccountId, dto.CategoryId, userId))
            {
                return BadRequest("Account or Category does not belong to the current user.");
            }

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Description = dto.Description,
                Date = dto.Date,
                IsIncome = dto.IsIncome,
                AccountId = dto.AccountId,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, UpdateTransactionDTO updateTransactionDTO)
        {
            var userId = GetUserId();
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                return NotFound();
            }
        
            if (!await AccountAndCategoryBelongToUser(updateTransactionDTO.AccountId, updateTransactionDTO.CategoryId, userId))
            {
                return BadRequest("Account or Category does not belong to the current user.");
            }

            transaction.Amount = updateTransactionDTO.Amount;
            transaction.Description = updateTransactionDTO.Description;
            transaction.Date = updateTransactionDTO.Date;
            transaction.IsIncome = updateTransactionDTO.IsIncome;
            transaction.AccountId = updateTransactionDTO.AccountId;
            transaction.CategoryId = updateTransactionDTO.CategoryId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TransactionExistsForUser(id, userId))
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
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = GetUserId();
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

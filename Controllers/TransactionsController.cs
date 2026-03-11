using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTrackerApi.Models;
using System.Threading.Tasks;

namespace MoneyTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly MoneyTrackerDbContext _context;

        public TransactionsController(MoneyTrackerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransactionById(int id)
        {
            return Ok(new { message = "Get transaction by id" });
        }

        [HttpPost]
        public IActionResult CreateTransaction()
        {
            return Ok(new { message = "Create new transaction" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTransaction()
        {
            return Ok(new { message = "Update transaction" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction()
        {
            return Ok(new { message = "Delete transaction" });
        }
    }
}

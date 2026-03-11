using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoneyTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllTransactions()
        {
            return Ok(new { message = "Get all transactions" });
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

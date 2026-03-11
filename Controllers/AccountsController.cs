using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoneyTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            return Ok(new {message = "Get all accounts"});
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

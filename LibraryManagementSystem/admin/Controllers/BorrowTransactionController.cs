using LibraryManagementSystem.admin.CommandModels;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.admin.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protect all endpoints
    public class BorrowTransactionController : ControllerBase
    {
        private readonly IBorrowTransactionService _service;

        public BorrowTransactionController(IBorrowTransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BorrowTransactionQueryModel>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowTransactionQueryModel>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound("Transaction not found.");
            return Ok(result);
        }

        [HttpPost("borrow")]
        public async Task<ActionResult> BorrowBook([FromBody] BorrowTransactionCommandModel model)
        {
            var success = await _service.BorrowAsync(model);
            if (!success) return BadRequest("Borrow failed. Book may be unavailable.");
            return Ok("Book borrowed successfully.");
        }

        [HttpPut("return")]
        public async Task<ActionResult> ReturnBook([FromBody] BorrowTransactionReturnModel model)
        {
            var success = await _service.ReturnAsync(model);
            if (!success) return BadRequest("Return failed. Transaction may not exist or book already returned.");
            return Ok("Book returned successfully.");
        }
    }
}

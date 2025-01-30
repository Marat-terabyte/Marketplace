using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Exceptions;
using PaymentService.Models;
using PaymentService.Models.InputModels;
using PaymentService.Models.Repositories;
using System.Security.Claims;

namespace PaymentService.Controllers.Api
{
    [ApiController]
    [Route("/api/balance")]
    public class BalanceController : Controller
    {
        private readonly IBalanceRepository _balanceRepository;

        public BalanceController(IBalanceRepository balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetBalanceAsync() 
        {
            var res = await CheckBalanceByUserId(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (!res.Item1)
                return NotFound();

            return Ok(res.Item2);
        }

        [HttpPost]
        [Route("topup")]
        public async Task<IActionResult> TopUpAsync([FromBody] Input input)
        {
            string? userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            try
            {
                await _balanceRepository.TopUpByUserIdAsync(userId, input.Amount);
                
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] Input input)
        {
            string? userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            return await Withdraw(userId, input.Amount);
        }

        private async Task<IActionResult> Withdraw(string userId, decimal amount)
        {
            try
            {
                await _balanceRepository.WithdrawByUserIdAsync(userId, amount);

                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (LowBalanceException)
            {
                return BadRequest();
            }
        }

        private async Task<(bool, Balance?)> CheckBalanceByUserId(string userId)
        {
            Balance? balance = await _balanceRepository.GetBalanceByUserIdAsync(userId);
            if (balance == null)
                return (false, null);

            return (true, balance);
        }
    }
}

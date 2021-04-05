using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskV3.Core.Dtos;
using TaskV3.Core.Interfaces.Business;

namespace TaskV3.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class DealerController : Controller
    {
        private readonly IDealerService _dealerService;

        public DealerController(IDealerService dealerService)
        {
            _dealerService = dealerService ?? throw new System.ArgumentNullException(nameof(dealerService));
        }

        /// <summary>
        /// Authenticate the specified name and password
        /// </summary>
        /// <returns>Jwt token</returns>
        /// <param name="name">Name.</param>
        /// <param name="password">Password.</param>
        [HttpGet]
        public async Task<IActionResult> AuthenticateUser(string name, string password)
        {
            try
            {
                var token = await _dealerService.AuthenticateAsync(name, password);
                if (token == null)
                {
                    return BadRequest(
                        new { message = "Username or password is incorrect" }
                    );
                }

                return Ok(token);
            }
            catch (Exception ex)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }
    }
}

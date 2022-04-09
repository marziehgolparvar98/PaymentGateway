using DataLayer.ViewModels.Authenticate;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Provider.Interface;
using PaymentGateway.Common;

namespace PaymentGateway.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticate;
        public AuthenticateController(IAuthenticateService authenticate)
        {
            _authenticate = authenticate;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginViewModel model)
        {
            var result =await _authenticate.LoginUserAsync(model);
            return result.ToHttpCodeResult();
        }

        [HttpPost("Regiter")]
        public async Task<IActionResult> RegiterUser([FromBody] RegisterViewModel model)
        {
            var result = await _authenticate.RegisterUserAsync(model);
            return result.ToHttpCodeResult();
        }
        [HttpPost]
        public IActionResult LogOutUser()
        {
            var result =  _authenticate.LogOutUserAsync();
            return result.ToHttpCodeResult();
        }

    }
}

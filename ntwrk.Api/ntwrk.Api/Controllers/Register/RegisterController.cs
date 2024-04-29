using ntwrk.Api.Controllers.Authenticate;

namespace ntwrk.Api.Controllers.Register
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        private IUserFunction _userFunction;

        public RegisterController(IUserFunction userFunction)
        {
            _userFunction = userFunction;
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterRequest request)
        {
            var response = _userFunction.Register(request.LoginId, request.UserName, request.Password);
            if (response == null)
                return BadRequest(new { StatusMessage = "something in the way..." });

            return Ok(response);
        }
    }
}

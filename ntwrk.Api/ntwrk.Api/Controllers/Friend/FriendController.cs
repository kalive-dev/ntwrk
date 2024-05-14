namespace ntwrk.Api.Controllers.AddFriend
{
    [ApiController]
    [Route("[controller]")]
    public class FriendController : Controller
    {
        private IUserFriendFunction _userFriendFunction;
        public FriendController(IUserFriendFunction userFriendFunction)
        {
            _userFriendFunction = userFriendFunction;
        }
        [HttpPost("AddFriend")]
        public IActionResult AddFriend(AddFriendRequest request)
        {
            var response = _userFriendFunction.AddFriend(request.userId, request.friendId);
            if (response == null)
                return BadRequest(new { StatusMessage = "addfriend response is null" });

            return Ok(response);
        }
    }
}

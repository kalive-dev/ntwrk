namespace ntwrk.Api.Controllers.Search
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IUserFunction _userFunction;
        public SearchController(IUserFunction userFunction)
        {
            _userFunction = userFunction;
        }
        [HttpPost("Search")]
        public async Task<IActionResult> Search(SearchRequest request)
        {
            var response = await _userFunction.Search(request.SearchRequestData);
            if (response == null)
                return BadRequest(new { StatusMessage = "something in the way..." });

            return Ok(response);

        }
    }
}

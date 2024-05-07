﻿namespace ntwrk.Api.Controllers.Search
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
        [HttpPost("GetUserById")]
        public IActionResult GetUserById(SearchRequest request)
        {
            var response = _userFunction.GetUserById((Convert.ToInt32(request.SearchRequestData)));
            if (response == null)
                return BadRequest();

            return Ok(response);

        }
    }
}

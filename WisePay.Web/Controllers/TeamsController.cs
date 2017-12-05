using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Teams;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Teams")]
    // [Authorize]
    public class TeamsController : Controller
    {
        private TeamsService _teamsService;

        public TeamsController(TeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateTeamModel model)
        {
            try
            {
                var teamId = await _teamsService.CreateTeam(model);
                return Created($"api/teams/{teamId}", new { Id = teamId });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError,
                    Message = e.Message
                });
            }
        }
        
        // GET: api/Teams
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Teams/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // PUT: api/Teams/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

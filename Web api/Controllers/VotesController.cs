using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class VotesController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var userId = GetUserId();
            if (!IsInScope())
            {
                return Unauthorized();
            }
            var userName = GetUserName();
            var voteResult = GetVoteResult(userId, userName);

            return Ok(new { res = voteResult });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Vote vote)
        {
            var userId = GetUserId();
            VoteRepository.SetVote(userId, vote.Value);
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        }
        private string GetUserName()
        {
            return HttpContext.User.FindFirst("name")?.Value;
        }
        private bool IsInScope()
        {
            var scopes = HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value;
            if (
                !string.IsNullOrEmpty(Scopes.Vote) &&
                scopes != null &&
                scopes.Split(' ').Any(s => s.Equals(Scopes.Vote))
            )
            {
                return true;
            }
            return false;
        }
        private string GetVoteResult(string userId, string userName)
        {
            var votes = VoteRepository.GetVotes(userId);
            var vanilleCount = votes.Count(s => s.Equals("Vanille", StringComparison.InvariantCultureIgnoreCase));
            var erdbeerCount = votes.Count(s => s.Equals("Erdbeer", StringComparison.InvariantCultureIgnoreCase));

            var result = $"{userName} hat sich noch nicht entschieden";
            if (vanilleCount > erdbeerCount)
            {
                result = $"{userName} mag eher Vanille. {vanilleCount}:{erdbeerCount}";
            }

            if (vanilleCount < erdbeerCount)
            {
                result = $"{userName} mag eher Erdbeer. {erdbeerCount}:{vanilleCount}";
            }

            if (vanilleCount != 0 && vanilleCount == erdbeerCount)
            {
                result = $"{userName} mag Erdbeer und Vanille. {erdbeerCount}:{vanilleCount}";
            }

            return result;
        }
    }
}
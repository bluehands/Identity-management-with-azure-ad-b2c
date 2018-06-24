using System.Collections.Generic;

namespace Web_api.Controllers
{
    public class VoteRepository
    {
        private static readonly Dictionary<string, List<string>> s_Votes = new Dictionary<string, List<string>>();

        public static List<string> GetVotes(string userId)
        {
            if (!s_Votes.TryGetValue(userId, out List<string> votes))
            {
                votes = new List<string>();
                s_Votes[userId] = votes;
            }

            return new List<string>(votes);
        }

        public static void SetVote(string userId, string value)
        {
            if (!s_Votes.TryGetValue(userId, out List<string> votes))
            {
                votes = new List<string>();
                s_Votes[userId] = votes;
            }

            votes.Add(value);
        }
    }
}
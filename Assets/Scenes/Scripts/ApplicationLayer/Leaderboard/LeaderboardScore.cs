namespace LegoBattaleRoyal.ApplicationLayer.Leaderboard
{
    public class LeaderboardScore
    {
        public string UserName { get; }
        public int UserScore { get; }

        public LeaderboardScore(string userName, int userScore)
        {
            UserName = userName;
            UserScore = userScore;
        }
    }
}
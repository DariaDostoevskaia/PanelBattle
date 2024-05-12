using Cysharp.Threading.Tasks;

namespace LegoBattaleRoyal.ApplicationLayer.Leaderboard
{
    public interface ILeaderboardProvider
    {
        UniTask AddScoreAsync(int score);

        UniTask<LeaderboardScore[]> GetScoresAsync();

        UniTask InitAsync();
    }
}
using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.ApplicationLayer.Leaderboard;

namespace LegoBattaleRoyal.Presentation.Controllers.Leaderboard
{
    
    public class LeaderboardController
    {
        private readonly ILeaderboardProvider _leaderboardProvider;

        public LeaderboardController(ILeaderboardProvider leaderboardProvider)
        {
            _leaderboardProvider = leaderboardProvider;
           
        }

        public UniTask AddScore(int score)
        {
            return _leaderboardProvider.AddScoreAsync(score);
        }

        public async UniTask PrintScores()
        {
            await _leaderboardProvider.GetScoresAsync();
        }

        public async UniTask InitAsync()
        {
            await _leaderboardProvider.InitAsync();
        }

        public void ShowLeaderboard()
        {
            //ShowLeaderboardUI();
        }
    }
}
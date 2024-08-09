using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.ApplicationLayer.Leaderboard;
using System.Linq;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace LegoBattaleRoyal.Infrastructure.Unity.Leaderboard
{
    public class UnityLeaderboardProvider : ILeaderboardProvider
    {
        private readonly string _leaderboardId = "PanelBattle";

        public async UniTask InitAsync()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            await UniTask.WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized);
        }

        public async UniTask AddScoreAsync(int score)
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardId, score);

            Debug.Log($"{scoreResponse.PlayerId}, {scoreResponse.PlayerName},{scoreResponse.Score}");
        }

        public async UniTask<LeaderboardScore[]> GetScoresAsync()
        {
            LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(_leaderboardId);

            return scoresResponse.Results
                .Select(score => new LeaderboardScore(score.PlayerName, (int)score.Score))
                .ToArray();
        }
    }
}
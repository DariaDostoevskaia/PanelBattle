using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.ApplicationLayer.Leaderboard;
using System;
using UnityEngine;

namespace LegoBattaleRoyal.Presentation.Controllers.Leaderboard
{
    public class LeaderboardController
    {
        private readonly ILeaderboardProvider _leaderboardProvider;
        private bool _isShowing;

        public LeaderboardController(ILeaderboardProvider leaderboardProvider)
        {
            _leaderboardProvider = leaderboardProvider;
        }

        public UniTask AddScore(int score)
        {
            return _leaderboardProvider.AddScoreAsync(score);
        }

        public async UniTask InitAsync()
        {
            await _leaderboardProvider.InitAsync();
        }

        public void ShowLeaderboard()
        {
            PrintScoresAsync().Forget();

            //ShowLeaderboardUI();
        }

        private async UniTask PrintScoresAsync()
        {
            if (_isShowing)
                return;

            _isShowing = true;
            try
            {
                var scores = await _leaderboardProvider.GetScoresAsync();
                if (scores.Length == 0)
                {
                    Debug.Log("Scores empty.");
                    return;
                }
                foreach (var score in scores)
                {
                    Debug.Log(score.ToString());
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                _isShowing = false;
            }
        }
    }
}
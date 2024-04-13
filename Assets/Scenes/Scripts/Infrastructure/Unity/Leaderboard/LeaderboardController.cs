using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using UnityEngine;

namespace LegoBattaleRoyal.Infrastructure.Unity.Leaderboard
{
    public class LeaderboardController
    {
        private readonly string _leaderboardId = "PanelBattle";  //вывести в лог

        private string VersionId { get; set; }

        private int Offset { get; set; }

        private int Limit { get; set; }

        public async void AddScore()
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardId, 102);

            Debug.Log($"{scoreResponse.PlayerId}, {scoreResponse.PlayerName},{scoreResponse.Score}");
        }

        public async void GetScores()
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(_leaderboardId);

            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPaginatedScores()
        {
            Offset = 10;
            Limit = 10;

            var scoresResponse = await LeaderboardsService.Instance
                .GetScoresAsync(_leaderboardId, new GetScoresOptions { Offset = Offset, Limit = Limit });

            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPlayerScore()
        {
            var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(_leaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }

        public async void GetVersionScores()
        {
            var versionScoresResponse = await LeaderboardsService.Instance.GetVersionScoresAsync(_leaderboardId, VersionId);

            Debug.Log(JsonConvert.SerializeObject(versionScoresResponse));
        }
    }
}
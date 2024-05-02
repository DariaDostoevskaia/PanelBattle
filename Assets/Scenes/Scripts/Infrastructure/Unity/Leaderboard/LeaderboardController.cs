using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace LegoBattaleRoyal.Infrastructure.Unity.Leaderboard
{
    public class LeaderboardController
    {
        private ILeaderboard _leaderboard;
        private readonly string _leaderboardId = "PanelBattle";  //вывести в лог

        private string VersionId { get; set; }

        private int Offset { get; set; }

        private int Limit { get; set; }

        public void Start()
        {
            // Authenticate user first
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    Debug.Log("Authentication successful");
                    string userInfo = "Username: " + Social.localUser.userName +
                        "\nUser ID: " + Social.localUser.id +
                        "\nIsUnderage: " + Social.localUser.underage;
                    Debug.Log(userInfo);
                }
                else
                    Debug.Log("Authentication failed");
            });

            // create social leaderboard
            _leaderboard = Social.CreateLeaderboard();
            _leaderboard.id = _leaderboardId;
        }

        public void ReportScore(long score)
        {
            Debug.Log("Reporting score " + score + " on leaderboard " + _leaderboardId);
            Social.ReportScore(score, _leaderboardId, success =>
            {
                Debug.Log(success
                    ? "Reported score successfully"
                    : "Failed to report score");
            });
        }

        public async UniTask Init()
        {
            await UnityServices.InitializeAsync();
        }

        public void ShowLeaderboard()
        {
            _leaderboard.LoadScores(result =>
            {
                Debug.Log("Received " + _leaderboard.scores.Length + " scores");
                foreach (IScore score in _leaderboard.scores)
                    Debug.Log(score);
            });
            GetScores();
            //Social.ShowLeaderboardUI();
        }

        public async void AddScore(int score)
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardId, score);

            Debug.Log($"{scoreResponse.PlayerId}, {scoreResponse.PlayerName},{scoreResponse.Score}");
        }

        public async void GetScores()
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(_leaderboardId);

            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        }

        public async void GetPaginatedScores()
        {
            Offset = 100;
            Limit = 100;

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
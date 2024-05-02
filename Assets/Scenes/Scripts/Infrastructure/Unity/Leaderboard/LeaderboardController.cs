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

        //void Start()
        //{
        //    ILeaderboard leaderboard = Social.CreateLeaderboard();
        //    leaderboard.id = _leaderboardId;
        //    leaderboard.LoadScores(result =>
        //    {
        //        Debug.Log("Received " + leaderboard.scores.Length + " scores");
        //        foreach (IScore score in leaderboard.scores)
        //            Debug.Log(score);
        //    });
        //}

        //private void Init()
        //{
        //    if (PlayGamesPlatform.Instance.IsAuthenticated())
        //    {
        //        Social.LoadScores("leaderboard_id", scores =>
        //    {
        //        if (scores.Length > 0)
        //        {
        //            string user = Social.localUser.id;

        //            foreach (IScore score in scores)
        //            {
        //                if (user == score.userID)
        //                {
        //                    rank_text.text = "YOUR RANK: " + score.rank.ToString();
        //                }
        //            }
        //        }
        //    });
        //    }
        //}

        //Social.ShowLeaderboardUI();
        //Social.LoadScores("Leaderboard ID", scores => {});

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
using Firebase;
using Firebase.Analytics;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using UnityEngine;

namespace LegoBattaleRoyal.Infrastructure.Firebase.Analytics
{
    public class FirebaseAnalyticksProvider : IAnalyticsProvider
    {
        private bool _isInit;

        public void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _isInit = true;
                    Debug.Log($"Resolve all Firebase dependencies: {dependencyStatus}");
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }

        //if (!levelController.TryBuyLevel(level.Price) /*&& PlayerPrefs.GetInt("StartInt", 3) */ )
        //{
        //    var button = generalPopup.CreateButton("Show Ads");
        //    button.onClick.AddListener(() =>
        //    {
        //        button.interactable = false;
        //        ShowRewardedAdsAsync().Forget();
        //    });
        //    generalPopup.SetTitle("Not enough energy.");
        //    generalPopup.SetText("There is not enough energy to buy the next level. Watch an advertisement to replenish energy.");

        //    generalPopup.Show();
        //    return;
        //}

        //async UniTask ShowRewardedAdsAsync()
        //{
        //    var result = await adsProvider.ShowRewarededAsync();
        //    generalPopup.Close();
        //    if (!result)
        //        return;

        //    levelController.EarnCoins(level.Price);

        //    StartGame();
        //}

        //private async UniTask DoSomething()
        //{
        //await UniTask.Init();
        //}

        public void SendEvent(string eventKey)
        {
            if (!_isInit)
                return;

            //TODO Init() async moment

            FirebaseAnalytics.LogEvent(eventKey);
            Debug.Log($"{GetType().Name} {nameof(SendEvent)} {eventKey}");
        }
    }
}
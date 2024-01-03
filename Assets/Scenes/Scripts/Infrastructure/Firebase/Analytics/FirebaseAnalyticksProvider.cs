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
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(async task =>
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

        //public async Task InitAsync()
        //{
        //    var tcs = new TaskCompletionSource<bool>();

        //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //    {
        //        var dependencyStatus = task.Result;
        //        if (dependencyStatus == DependencyStatus.Available)
        //        {
        //            _isInit = true;
        //            Debug.Log($"Resolve all Firebase dependencies: {dependencyStatus}");
        //            tcs.SetResult(true);
        //        }
        //        else
        //        {
        //            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        //            tcs.SetResult(false);
        //        }
        //    });

        //    // Ожидание изменения значения _isInit на true
        //    await tcs.Task;

        //    // Вызов следующего метода после инициализации
        //    // TODO: Call the next method
        //}

        //public async UniTask InitAsync()
        //{
        //    var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        //    if (dependencyStatus == DependencyStatus.Available)
        //    {
        //        _isInit = true;
        //        Debug.Log($"Resolve all Firebase dependencies: {dependencyStatus}");
        //        NextMethod(); // Вызов следующего метода после инициализации
        //    }
        //    else
        //    {
        //        Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        //    }
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
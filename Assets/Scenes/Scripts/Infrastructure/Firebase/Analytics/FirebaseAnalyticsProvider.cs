using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using UnityEngine;

namespace LegoBattaleRoyal.Infrastructure.Firebase.Analytics
{
    public class FirebaseAnalyticsProvider : IAnalyticsProvider
    {
        private bool _isInit;

        public async UniTask InitAsync()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                _isInit = true;
                Debug.Log($"Resolve all Firebase dependencies: {dependencyStatus}");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        }

        public void SendEvent(string eventKey)
        {
            if (!_isInit)
                return;

            FirebaseAnalytics.LogEvent(eventKey);
            Debug.Log($"{GetType().Name} {nameof(SendEvent)} {eventKey}");
        }
    }
}
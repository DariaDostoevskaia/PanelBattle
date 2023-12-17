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
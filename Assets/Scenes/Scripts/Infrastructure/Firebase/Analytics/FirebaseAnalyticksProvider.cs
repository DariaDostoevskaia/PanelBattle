using Firebase;

namespace LegoBattaleRoyal.Infrastructure.Firebase.Analytics
{
    public class FirebaseAnalyticksProvider
    {
        public void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    UnityEngine.Debug.Log($"Resolve all Firebase dependencies: {dependencyStatus}");
                }
                else
                {
                    UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }
    }
}
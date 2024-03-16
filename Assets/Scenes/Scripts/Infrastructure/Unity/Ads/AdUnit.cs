using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace LegoBattaleRoyal.Infrastructure.Unity.Ads
{
    public class AdUnit : IUnityAdsLoadListener, IUnityAdsShowListener, IDisposable
    {
        public event Action<bool> OnLoaded;

        public event Action OnSuccesShown;

        public event Action OnFailedShown;

        private readonly string _adUnitId;

        public bool IsReady { get; private set; }

        public AdUnit(string adUnitId)
        {
            _adUnitId = adUnitId;
        }

        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(_adUnitId))
            {
                IsReady = true;
                OnLoaded?.Invoke(true);
            }
        }

        // Implement a method to execute when the user clicks the button:
        public void ShowAd()
        {
            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (!adUnitId.Equals(_adUnitId))
                return;

            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                OnSuccesShown?.Invoke();
                Debug.Log("Unity Ads Rewarded Ad Completed");
            }
            else
            {
                OnFailedShown?.Invoke();
            }

            ReLoad();
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            if (!adUnitId.Equals(_adUnitId))
                return;

            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.

            ReLoad();
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            if (!adUnitId.Equals(_adUnitId))
                return;

            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.

            ReLoad();
        }

        private void ReLoad()
        {
            IsReady = false;
            OnLoaded?.Invoke(false);
            LoadAd();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Start show unity ads {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"Click unity ads {placementId}");
        }

        public void Dispose()
        {
            OnLoaded = null;
            OnFailedShown = null;
            OnSuccesShown = null;
        }
    }
}
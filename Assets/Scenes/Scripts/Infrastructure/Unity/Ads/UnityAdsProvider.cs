using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace LegoBattaleRoyal.Infrastructure.Unity.Ads
{
    public class UnityAdsProvider : IUnityAdsInitializationListener, IDisposable
    {
        public event Action<bool> OnUnityAdsLoaded;

        private static readonly string _androidGameId = "5493303";
        private static readonly string _iOSGameId = "5493302";
#if UNITY_IOS
        private static readonly string _rewardedPlacementId = "Rewarded_iOS" ;
        private static readonly string _intrestitialPlacementId = "Interstitial_Android";
#elif UNITY_ANDROID || UNITY_EDITOR
        private static readonly string _rewardedPlacementId = "Rewarded_Android";
        private static readonly string _intrestitialPlacementId = "Interstitial_iOS";
#endif
        private bool _testMode;
        private bool _wait = true;
        private bool _result = false;

        private readonly AdUnit _rewardedPlacement;
        private readonly AdUnit _intrestitialPlacement;

        public UnityAdsProvider(/*analyticsProvider*/)
        {
            _rewardedPlacement = new AdUnit(_rewardedPlacementId);
            _rewardedPlacement.OnLoaded += OnAdsLoaded;

            _intrestitialPlacement = new AdUnit(_intrestitialPlacementId);
            _intrestitialPlacement.OnLoaded += OnAdsLoaded;
        }

        private void OnUnityAdsShow()
        {
            if (!_testMode)
                return;
        }

        public async UniTask<bool> ShowRewarededAsync()
        {
            _rewardedPlacement.OnFailedShown += RewardedFailedShown;
            _rewardedPlacement.OnSuccesShown += RewardedSuccesShown;

            ShowRewarded();

            await UniTask.WaitWhile(() => _wait);
            return _result;
        }

        public async UniTask<bool> ShowIntrestitialAsync()
        {
            _intrestitialPlacement.OnFailedShown += InterstitialFailedShown;
            _intrestitialPlacement.OnSuccesShown += InterstitialSuccesShown;

            ShowInterstitial();

            await UniTask.WaitWhile(() => _wait);
            return _result;
        }

        private void RewardedFailedShown()
        {
            OnFailedShown();
            //analyticsProvider.SendEvent(AnalyticsEvents.RewardedError);
        }

        private void RewardedSuccesShown()
        {
            OnSuccesShown();
            //analyticsProvider.SendEvent(AnalyticsEvents.RewardedSucces);
        }

        private void InterstitialFailedShown()
        {
            OnFailedShown();
            //analyticsProvider.SendEvent(AnalyticsEvents.InterstitialError);
        }

        private void InterstitialSuccesShown()
        {
            OnSuccesShown();
            //analyticsProvider.SendEvent(AnalyticsEvents.InterstitialSucces);
        }

        private void OnSuccesShown()
        {
            _result = true;

            EndShow();
        }

        private void OnFailedShown()
        {
            EndShow();
        }

        private void EndShow()
        {
            _rewardedPlacement.OnFailedShown -= RewardedFailedShown;
            _rewardedPlacement.OnSuccesShown -= RewardedSuccesShown;

            _intrestitialPlacement.OnFailedShown -= InterstitialFailedShown;
            _intrestitialPlacement.OnSuccesShown -= InterstitialSuccesShown;

            _wait = false;
        }

        private void ShowRewarded()
        {
            _rewardedPlacement.ShowAd();
            //analyticsProvider.SendEvent(AnalyticsEvents.ShowRewarded);
        }

        private void ShowInterstitial()
        {
            _intrestitialPlacement.ShowAd();
            //analyticsProvider.SendEvent(AnalyticsEvents.ShowInterstitial);
        }

        private void OnAdsLoaded(bool isLoaded)
        {
            OnUnityAdsLoaded?.Invoke(isLoaded);
        }

        public void InitializeAds()
        {
#if DEBUG
            _testMode = true;
#endif

#if UNITY_IOS
            var gameId = _iOSGameId;
#elif UNITY_ANDROID || UNITY_EDITOR
            var gameId = _androidGameId;
#endif
            if (!Advertisement.isInitialized
                && Advertisement.isSupported)
            {
                Advertisement.Initialize(gameId, _testMode, this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void Dispose()
        {
            OnUnityAdsLoaded = null;

            _rewardedPlacement.OnLoaded -= OnAdsLoaded;
            _rewardedPlacement.OnSuccesShown -= OnUnityAdsShow;

            _intrestitialPlacement.OnLoaded -= OnAdsLoaded;
            _intrestitialPlacement.OnSuccesShown -= OnUnityAdsShow;

            _rewardedPlacement.Dispose();
            _intrestitialPlacement.Dispose();
        }
    }
}
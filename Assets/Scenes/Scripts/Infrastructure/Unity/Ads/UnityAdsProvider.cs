using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.ApplicationLayer.Analytics;
using LegoBattaleRoyal.Infrastructure.Firebase.Analytics;
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
        private readonly FirebaseAnalyticsProvider _analyticsProvider;

        public bool IsRewardedSuccesShown { get; private set; }

        public bool IsIntrestitialSuccesShown { get; private set; }

        public UnityAdsProvider(Firebase.Analytics.FirebaseAnalyticsProvider analyticsProvider)
        {
            _rewardedPlacement = new AdUnit(_rewardedPlacementId);
            _rewardedPlacement.OnLoaded += OnAdsLoaded;

            _intrestitialPlacement = new AdUnit(_intrestitialPlacementId);
            _intrestitialPlacement.OnLoaded += OnAdsLoaded;

            _analyticsProvider = analyticsProvider;
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
            _analyticsProvider.SendEvent(AnalyticsEvents.RewardedError);
            IsRewardedSuccesShown = false;
        }

        private void RewardedSuccesShown()
        {
            OnSuccesShown();
            _analyticsProvider.SendEvent(AnalyticsEvents.RewardedSucces);
            IsRewardedSuccesShown = true;
        }

        private void InterstitialFailedShown()
        {
            OnFailedShown();
            _analyticsProvider.SendEvent(AnalyticsEvents.InterstitialError);
            IsIntrestitialSuccesShown = false;
        }

        private void InterstitialSuccesShown()
        {
            OnSuccesShown();
            _analyticsProvider.SendEvent(AnalyticsEvents.InterstitialSucces);
            IsIntrestitialSuccesShown = true;
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
            _analyticsProvider.SendEvent(AnalyticsEvents.ShowRewarded);
        }

        private void ShowInterstitial()
        {
            _intrestitialPlacement.ShowAd();
            _analyticsProvider.SendEvent(AnalyticsEvents.ShowInterstitial);
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
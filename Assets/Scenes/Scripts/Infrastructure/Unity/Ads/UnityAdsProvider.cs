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

        private readonly AdUnit _rewardedPlacement;
        private readonly AdUnit _intrestitialPlacement;
        private readonly FirebaseAnalyticsProvider _analyticsProvider;

        public UnityAdsProvider()
        {
            _rewardedPlacement = new AdUnit(_rewardedPlacementId);
            _rewardedPlacement.OnLoaded += OnAdsLoaded;

            _intrestitialPlacement = new AdUnit(_intrestitialPlacementId);
            _intrestitialPlacement.OnLoaded += OnAdsLoaded;

            _analyticsProvider = analyticsProvider;
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

        public async UniTask<bool> ShowRewarededAsync()
        {
            _rewardedPlacement.OnFailedShown += OnFailedShown;
            _rewardedPlacement.OnSuccesShown += OnSuccesShown;

            var wait = true;
            var result = false;

            ShowRewarded();

            await UniTask.WaitWhile(() => wait);
            return result;

            void OnSuccesShown()
            {
                result = true;
                EndShow();
            }

            void OnFailedShown()
            {
                EndShow();
            }

            void EndShow()
            {
                _rewardedPlacement.OnFailedShown -= OnFailedShown;
                _rewardedPlacement.OnSuccesShown -= OnSuccesShown;
                wait = false;
            }
        }

        public void ShowInterstitial()
        {
            _intrestitialPlacement.OnFailedShown += InterstitialFailedShown;
            _intrestitialPlacement.OnSuccesShown += InterstitialSuccesShown;

            _intrestitialPlacement.ShowAd();

            void InterstitialSuccesShown()
            {
                EndIntrestitialShow();
            }

            void InterstitialFailedShown()
            {
                EndIntrestitialShow();
            }

            void EndIntrestitialShow()
            {
                _intrestitialPlacement.OnSuccesShown -= InterstitialSuccesShown;
                _intrestitialPlacement.OnFailedShown -= InterstitialFailedShown;
            }
        }

        private void ShowRewarded()
        {
            _rewardedPlacement.ShowAd();
        }

        private void OnAdsLoaded(bool isLoaded)
        {
            OnUnityAdsLoaded?.Invoke(isLoaded);
        }

        private void OnUnityAdsShow()
        {
            if (!_testMode)
                return;
        }

        public void Dispose()
        {
            OnUnityAdsLoaded = null;

            _rewardedPlacement.OnLoaded -= OnAdsLoaded;
            _intrestitialPlacement.OnLoaded -= OnAdsLoaded;

            _rewardedPlacement.Dispose();
            _intrestitialPlacement.Dispose();
        }
    }
}
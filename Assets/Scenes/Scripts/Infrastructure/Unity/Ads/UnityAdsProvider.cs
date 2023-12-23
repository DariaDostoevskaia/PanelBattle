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

        private AdUnit _rewardedPlacement;
        private AdUnit _intrestitialPlacement;

        public UnityAdsProvider()
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

            if (!_rewardedPlacement.IsReady)
            {
                //выкл интерактивность кнопки
                return;
            }
            if (!_intrestitialPlacement.IsReady)
            {
                //выкл интерактивность кнопки
                return;
            }
            //на его подписать ui и выдать игроку монеты
            //если реклама не доступна - откл кнопку
            //если из гл меню - не
        }

        public void ShowRewareded()
        {
            _rewardedPlacement.ShowAd();
        }

        public async UniTask<bool> ShowRewarededAsync()
        {
            _rewardedPlacement.OnFailedShown += OnFailedShown;
            _rewardedPlacement.OnSuccesShown += OnSuccesShown;

            var wait = true;
            var result = false;

            _rewardedPlacement.ShowAd();

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
            _intrestitialPlacement.ShowAd();
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
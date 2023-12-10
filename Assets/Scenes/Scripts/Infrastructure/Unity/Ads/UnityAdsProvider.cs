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
#elif UNITY_ANDROID || UNITY_EDITOR
        private static readonly string _rewardedPlacementId = "Rewarded_Android";
#endif
        private bool _testMode;
        private AdUnit _rewardedPlacement;

        public UnityAdsProvider()
        {
            _rewardedPlacement = new AdUnit(_rewardedPlacementId);
            _rewardedPlacement.OnLoaded += OnAdsLoaded;

            _rewardedPlacement.OnSuccesShown += OnUnityAdsShow;

            // �������� ������������� ������� intrestitial

            //_rewardedPlacement.OnFailed+= ?
        }

        private void OnUnityAdsShow()
        {
            //�� ��� ��������� ui � ������ ������ ������
            //���� ������� �� �������� - ���� ������
            //���� �� �� ���� - ��
        }

        public void ShowRewareded()
        {
            _rewardedPlacement.ShowAd();
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
            if (!Advertisement.isInitialized && Advertisement.isSupported)
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

            _rewardedPlacement.Dispose();
        }
    }
}
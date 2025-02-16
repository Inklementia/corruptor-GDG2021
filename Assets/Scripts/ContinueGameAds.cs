using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class ContinueGameAds : MonoBehaviour
{
#if UNITY_IOS
    private const string GameId = "4198042";
#else
    private const string GameId = "4198043";
#endif

    private const string PlacementId = "ContinueGame";
    private Action _onRewardedAdSuccess;
    private bool _isAdLoaded = false;

    [SerializeField] private Canvas pauseButtonCanvas;

    private void Start()
    {
        Advertisement.Initialize(GameId, false, new InitializationCallback(this));
    }

    public void PlayContinueGameAd(Action onSuccess)
    {
        _onRewardedAdSuccess = onSuccess;

        if (_isAdLoaded)
        {
            Advertisement.Show(PlacementId, new AdShowCallback(this));
        }
        else
        {
            Debug.Log("ADS ARE NOT READY");
        }
    }

    private class InitializationCallback : IUnityAdsInitializationListener
    {
        private readonly ContinueGameAds _adsManager;

        public InitializationCallback(ContinueGameAds adsManager)
        {
            _adsManager = adsManager;
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads Initialized.");
            Advertisement.Load(PlacementId, new AdLoadCallback(_adsManager));
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }

    private class AdLoadCallback : IUnityAdsLoadListener
    {
        private readonly ContinueGameAds _adsManager;

        public AdLoadCallback(ContinueGameAds adsManager)
        {
            _adsManager = adsManager;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId == PlacementId)
            {
                Debug.Log($"Ad {placementId} loaded successfully.");
                _adsManager._isAdLoaded = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Ad Load Failed: {placementId}, Error: {error}, Message: {message}");
        }
    }

    private class AdShowCallback : IUnityAdsShowListener
    {
        private readonly ContinueGameAds _adsManager;

        public AdShowCallback(ContinueGameAds adsManager)
        {
            _adsManager = adsManager;
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showResult)
        {
            if (placementId == PlacementId)
            {
                if (showResult == UnityAdsShowCompletionState.COMPLETED)
                {
                    Debug.Log("VIDEO FINISHED");
                    _adsManager._onRewardedAdSuccess?.Invoke();
                    _adsManager.pauseButtonCanvas.enabled = true;
                }
                else if (showResult == UnityAdsShowCompletionState.SKIPPED)
                {
                    Debug.Log("Ad skipped - no reward");
                }
                else if (showResult == UnityAdsShowCompletionState.UNKNOWN)
                {
                    Debug.LogError("Ad failed to show.");
                }

                // Reload the ad for future use
                Advertisement.Load(PlacementId, new AdLoadCallback(_adsManager));
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Ad Show Failed: {placementId}, Error: {error}, Message: {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("Ad started.");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log("Ad clicked.");
        }
    }
}

using System;
using GoogleMobileAds.Api;
using UnityEngine;
using VitaliyNULL.Managers;

namespace VitaliyNULL.GoogleAds
{
    public class GoogleAdsManager : MonoBehaviour
    {
        private RewardedAd rewardedAd;
        public static GoogleAdsManager Instance;
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Instance = this;
            }

            MobileAds.Initialize((InitializationStatus initStatus) => { Debug.Log("Ad is initialized"); });
            LoadRewardedAd();
        }

        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                    RegisterReloadHandler(rewardedAd);
                });
        }


        public void ShowRewardedAd()
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show(reward =>
                {
#if UNITY_EDITOR
                    GameManager.Instance.ContinueGame();
#endif
                    Debug.Log("Show rewarded ad");
                });
            }
        }


        private void RegisterReloadHandler(RewardedAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
            ad.OnAdFullScreenContentOpened += () => Debug.Log("Rewarded ad full screen opened");
            ad.OnAdPaid += value =>
            {
                Debug.Log("Get reward");
                GameManager.Instance.ContinueGame();
            };
        }
    }
}
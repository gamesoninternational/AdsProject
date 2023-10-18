using System;
using UnityEngine;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class MyGoogleAdsV2 : MonoBehaviour
{
    public static MyGoogleAdsV2 instance;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private void Awake()
    {
        instance = this;
    }
        string REWARDED_adUnitId;
        string BANNER_adUnitId;
        string INTERSTITIAL_adUnitId;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_ANDROID
            REWARDED_adUnitId = "ca-app-pub-3949237800163961/8179663997";
            BANNER_adUnitId = "ca-app-pub-3082647315282744/8063446677";
            INTERSTITIAL_adUnitId = "";

#elif UNITY_IPHONE
            REWARDED_adUnitId = "";
        BANNER_adUnitId = "";
        INTERSTITIAL_adUnitId = "";
#else
             adUnitId = "unexpected_platform";
#endif
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        List<string> deviceIds = new List<string>();
        deviceIds.Add("5E8A8D8FC350236DCD8AEB91725EDD33"); //S20
        deviceIds.Add("0A38B5FD4A88AA93ADBB291F670F06FB"); //A01
        deviceIds.Add("96D8EB0A983014677936023FA075D1B6"); //A02
        deviceIds.Add("1E4A1B03D1B6CD8A174A826F76E009F4"); //K50
        deviceIds.Add("D45837DF75647B894B9323D51A5B8FB"); //mi mix
        deviceIds.Add("05DA7EBCC164B9E9A1655A384C462659"); //mi mix
        RequestConfiguration requestConfiguration = new RequestConfiguration
                .Builder()
                .SetTestDeviceIds(deviceIds)
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);


            MobileAds.Initialize((initStatus) =>
            {
                Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
                {
                    string className = keyValuePair.Key;
                    AdapterStatus status = keyValuePair.Value;
                    switch (status.InitializationState)
                    {
                        case AdapterState.NotReady:
                            // The adapter initialization did not complete.
                            MonoBehaviour.print("Adapter: " + className + " not ready.");
                            break;
                        case AdapterState.Ready:
                            // The adapter was successfully initialized.
                            MonoBehaviour.print("Adapter: " + className + " is initialized.");
                            break;
                    }
                }

                CreateAndLoadRewardedAd();
                RequestInterstitial();
                //RequestBanner(AdPosition.Bottom, AdSize.Banner);
            });

        

        }
    public static Action _succeedCallBack;

    bool rewarded;
        bool closed;
        void Update()
        {
            if (rewarded)
            {
                rewarded = false;

                if (_succeedCallBack != null)
                {
                    _succeedCallBack();
                    _succeedCallBack = null;
                }
            }
            if (closed)
            {
                closed = false;
            }
        }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    public void CreateAndLoadRewardedAd()
        {

        // create new rewarded ad instance
        RewardedAd.Load(REWARDED_adUnitId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    return;
                }
                else if (ad == null)
                {
                    return;
                }

                rewardedAd = ad;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Rewarded ad was OnAdFullScreenContentOpened.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Rewarded ad was OnAdFullScreenContentClosed.");
                    closed = true;
                    rewarded = true;
                    CreateAndLoadRewardedAd();


                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("Rewarded ad was OnAdImpressionRecorded.");
                };
                ad.OnAdClicked += () =>
                {
                    Debug.Log("Rewarded ad was OnAdClicked.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {

                    Debug.Log("Rewarded ad was OnAdFullScreenContentFailed.");
                    CreateAndLoadRewardedAd();

                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    Debug.Log("Rewarded ad was OnAdPaid.");
                    rewarded = true;
                    closed = true;
                };
            });



        }
        public void ShowRewardedAd()
        {
            if (this.rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                });
        }
        }
    public bool CheckHasRewardedAd()
    {
        if (this.rewardedAd.CanShowAd())
        {
            return true;
        }
        CreateAndLoadRewardedAd();
        return false;
    }



        private void RequestInterstitial()
        {
#if UNITY_IPHONE
        return;
#endif
        // Clean up interstitial before using it
        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        // Load an interstitial ad
        InterstitialAd.Load(INTERSTITIAL_adUnitId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    return;
                }
                else if (ad == null)
                {
                    return;
                }

                interstitial = ad;

                ad.OnAdFullScreenContentOpened += () =>
                {
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    rewarded = true;
                    closed = true;
                    RequestInterstitial();
                };
                ad.OnAdImpressionRecorded += () =>
                {
                };
                ad.OnAdClicked += () =>
                {
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    RequestInterstitial();

                };
                ad.OnAdPaid += (AdValue adValue) =>
                {

                };
            });

        }
        public bool CheckHasInterstitialAd()
        {
#if UNITY_IPHONE
        return false;
#endif
        if (this.interstitial.CanShowAd())
                return true;

        RequestInterstitial();
        return false;
    }

        public void ShowInterstitialAd()
        {
#if UNITY_IPHONE
        return;
#endif
        if (this.interstitial.CanShowAd())
            {
                this.interstitial.Show();
            }
        }

    #region BANNER ADS
    public void OnClickShowBanner() {
        RequestBannerAd(AdPosition.Bottom, AdSize.Banner);
    }
    public void RequestBannerAd(AdPosition _pos, AdSize _size)
    {


        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(BANNER_adUnitId, _size, _pos);

        // Add Event Handlers
        bannerView.OnBannerAdLoaded += () =>
        {
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
        };
        bannerView.OnAdClicked += () =>
        {
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {

        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
    public void ShowBanner(AdPosition _pos, AdSize _size)
    {

        RequestBannerAd(_pos, _size);
    }
    public void HideBanner()
    {
        Debug.LogError("HideBanner");

        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
    #endregion
    public void ShowDebbuger() {
        MobileAds.OpenAdInspector(error => {
            // Error will be set if there was an issue and the inspector was not displayed.
        });
    }


    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            try
            {

            }
            catch
            {

            }
        }
        else
        {
            try
            {
                if (!this.rewardedAd.CanShowAd())
                {
                    CreateAndLoadRewardedAd();
                }
                if (!this.interstitial.CanShowAd())
                {
                    RequestInterstitial();
                }
            }
            catch
            {

            }

        }
    }

}

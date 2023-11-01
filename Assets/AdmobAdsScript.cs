using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class AdmobAdsScript : MonoBehaviour
{
    //public TextMeshProUGUI totalCoinsTxt;
    //public GameObject LoadingPanel;
    public TextMeshProUGUI TextInfoUI;
    public Button ButtonAds;
    public string appId = "ca-app-pub-3949237800163961~7081373850";// "ca-app-pub-3940256099942544~3347511713";

#if UNITY_EDITOR
    string bannerId = "unused";
    string interId = "unused";
    string rewardedId = "unused";
    string nativeId = "unused";

#elif UNITY_ANDROID
    string bannerId = "ca-app-pub-3949237800163961/6373572398";
    string interId = "ca-app-pub-3949237800163961/9494559756";
    string rewardedId = "ca-app-pub-3949237800163961/8179663997";
    string nativeId = "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_IPHONE
    string bannerId = "ca-app-pub-3940256099942544/2934735716";
    string interId = "ca-app-pub-3940256099942544/4411468910";
    string rewardedId = "ca-app-pub-3940256099942544/1712485313";
    string nativeId = "ca-app-pub-3940256099942544/3986624511";
#endif

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;
    //NativeAd nativeAd;

    #region Listener Ad
    [Header("Banner Ad Listener")]
    public BannerAdEvent bannerAdEvent;
    [Header("Rewarded Ad Listener")]
    public RewardedAdEvent rewardedAdEvent;
    [Header("Interstitial Ad Listener")]
    public InterstitalAdEvent interstitalAdEvent;
    #endregion

    private void Start()
    {
        ShowCoins();
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {

            print("Ads Initialised !!");
            ButtonAds.interactable = false;
            TextInfoUI.text = "Admob Loading";
        });
        LoadRewardedAd();
        
    }

    #region Banner

    public void LoadBannerAd()
    {
        //create a banner
        CreateBannerView();

        //listen to banner events
        ListenToBannerEvents();

        //load the banner
        if (bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        bannerView.LoadAd(adRequest);//show the banner on the screen
    }
    void CreateBannerView()
    {

        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
    }

    void ListenToBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());

            bannerAdEvent.OnBannerAdLoaded.Invoke();
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);

            bannerAdEvent.OnBannerAdLoadFailed.Invoke(error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);

            bannerAdEvent.OnAdPaid.Invoke(adValue);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");

            bannerAdEvent.OnAdImpressionRecorded.Invoke();
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");

            bannerAdEvent.OnAdClicked.Invoke();
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");

            bannerAdEvent.OnAdFullScreenContentOpened.Invoke();
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");

            bannerAdEvent.OnAdFullScreenContentOpened?.Invoke();
        };
    }

    public void DestroyBannerAd()
    {

        if (bannerView != null)
        {
            print("Destroying banner Ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion


    #region Interstitial

    public void LoadInterstitialAd()
    {

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial ad failed to load" + error);
                return;
            }

            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });

    }
    public void ShowInterstitialAd()
    {

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            print("Intersititial ad not ready!!");
        }
    }
    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);

            interstitalAdEvent.OnAdPaid.Invoke(adValue);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");

            interstitalAdEvent.OnAdImpressionRecorded.Invoke();
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");

            interstitalAdEvent.OnAdClicked.Invoke();
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");

            interstitalAdEvent.OnAdFullScreenContentOpened.Invoke();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");

            interstitalAdEvent.OnAdFullScreenContentClosed.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            interstitalAdEvent.OnAdFullScreenContentFailed.Invoke(error);
        };
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Rewarded failed to load" + error);
                return;
            }

            print("Rewarded ad loaded !!");
            ButtonAds.interactable = true;
            TextInfoUI.text = "Admob Rewarded ad loaded !!";
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
        });
    }
    public void ShowRewardedAd()
    {
        //LoadingPanel.SetActive(true);
        ButtonAds.interactable = false;
        TextInfoUI.text = "Admob Loading";
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give reward to player !!");

                rewardedAdEvent.OnGetReward.Invoke();

            });
        }
        else
        {
            print("Rewarded ad not ready");
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);

            rewardedAdEvent.OnAdPaid.Invoke(adValue);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");

            rewardedAdEvent.OnAdImpressionRecorded.Invoke();
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");

            rewardedAdEvent.OnAdClicked.Invoke();
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");

            rewardedAdEvent.OnAdFullScreenContentOpened.Invoke();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            //ButtonAds.interactable = false;
            //LoadingPanel.SetActive(false);
            LoadRewardedAd();
            
            rewardedAdEvent.OnAdFullScreenContentClosed.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            rewardedAdEvent.OnAdFullScreenContentFailed.Invoke(error);
        };
    }



    #endregion


    #region Native

    //public Image img;

    /*public void RequestNativeAd() {

        AdLoader adLoader = new AdLoader.Builder(nativeId).ForNativeAd().Build();

        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

        adLoader.LoadAd(new AdRequest.Builder().Build());
    }*/

    /*private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
        print("Native ad loaded");
        this.nativeAd = e.nativeAd;

        Texture2D iconTexture = this.nativeAd.GetIconTexture();
        Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.one * .5f);

        img.sprite = sprite;

    }*/

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        print("Native ad failed to load" + e.ToString());

    }
    #endregion


    #region extra 

    public void GrantCoins(int coins)
    {
        int crrCoins = PlayerPrefs.GetInt("totalCoins");
        crrCoins += coins;
        PlayerPrefs.SetInt("totalCoins", crrCoins);

        ShowCoins();
    }
    void ShowCoins()
    {
        //totalCoinsTxt.text = PlayerPrefs.GetInt("totalCoins").ToString();
    }

    #endregion

    #region CLASS Ad Event Listener


    #region UnityEventsAd
    [Serializable]
    public class RewardedAdEvent
    {
        public UnityEvent OnGetReward;
        public UnityEvent<AdValue> OnAdPaid;
        public UnityEvent OnAdImpressionRecorded;
        public UnityEvent OnAdClicked;
        public UnityEvent OnAdFullScreenContentOpened;
        public UnityEvent OnAdFullScreenContentClosed;
        public UnityEvent<AdError> OnAdFullScreenContentFailed;
    }

    [Serializable]
    public class BannerAdEvent
    {
        public UnityEvent OnBannerAdLoaded;
        public UnityEvent<LoadAdError> OnBannerAdLoadFailed;
        public UnityEvent<AdValue> OnAdPaid;
        public UnityEvent OnAdImpressionRecorded;
        public UnityEvent OnAdClicked;
        public UnityEvent OnAdFullScreenContentOpened;
        public UnityEvent OnAdFullScreenContentClosed;
    }

    [Serializable]
    public class InterstitalAdEvent
    {
        public UnityEvent<AdValue> OnAdPaid;
        public UnityEvent OnAdImpressionRecorded;
        public UnityEvent OnAdClicked;
        public UnityEvent OnAdFullScreenContentOpened;
        public UnityEvent OnAdFullScreenContentClosed;
        public UnityEvent<AdError> OnAdFullScreenContentFailed;
    }

    #endregion
    #endregion

}

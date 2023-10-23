using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public GameObject AdsScript;
    public int Ads;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAds(){
        Ads++;
        if (Ads == 2){
            Ads = 0;
            AdsScript.GetComponent<IronSourceDemoScript>().ShowRewardedAdsIS();
            Debug.Log("ISAds");
        }
        if (Ads == 1){
            AdsScript.GetComponent<AdmobAdsScript>().ShowRewardedAd();
            Debug.Log("AdmobAds");
        }
    }
}

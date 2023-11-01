using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdsManager : MonoBehaviour
{
    public GameObject AdsScript;
    public int AdsInt;
    public TextMeshProUGUI textUI, TextInfoUI;
    public string TextInfo;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = AdsInt.ToString();
        //TextInfoUI.text = "Test";
        //TextInfoUI.GetComponent<TextMeshProUGUI>().text = TextInfo;
    }

    public void ShowAds(){
        AdsInt++;
        if (AdsInt == 2){
            AdsInt = 0;
            AdsScript.GetComponent<IronSourceDemoScript>().ShowRewardedAdsIS();
            Debug.Log("ISAds");
            
        }
        if (AdsInt == 1){
            AdsScript.GetComponent<AdmobAdsScript>().ShowRewardedAd();
            Debug.Log("AdmobAds");
            
        }
    }

    public void ShowAdsIS()
    {
   
        AdsScript.GetComponent<IronSourceDemoScript>().ShowRewardedAdsIS();
    
        
    }
}

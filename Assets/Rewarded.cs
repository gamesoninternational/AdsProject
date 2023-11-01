using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rewarded : MonoBehaviour
{
    public string appkey;
    public TextMeshProUGUI TextInfoUI;
    // Start is called before the first frame update
    void Start()
    {
        IronSource.Agent.shouldTrackNetworkState(true);
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rewarded()
    {
        
        IronSource.Agent.showRewardedVideo();
        TextInfoUI.text = "IS Show";
    }

    void RewardedVideoAdClosedEvent()
    {
        IronSource.Agent.init(appkey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);
        TextInfoUI.text = "IS Closed";
    }

    void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        //Change the in-app 'Traffic Driver' state according to availability.
        bool rewardedVideoAvailability = available;
        TextInfoUI.text = "IS Loaded";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializeads : MonoBehaviour
{

    public string appKey;

    // Start is called before the first frame update

    private void Awake()
    {
        IronSource.Agent.init(appKey);
    }

    void Start()
    {
        Loadbanner();
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void Loadbanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }
}

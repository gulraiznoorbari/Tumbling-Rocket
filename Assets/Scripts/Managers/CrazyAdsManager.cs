using System;
using System.Collections;
using CrazyGames;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class CrazyAdsManager : MonoBehaviour
{
    public static CrazyAdsManager Instance;

    [SerializeField] private CrazyBanner banner;
    // [SerializeField] private int nextSceneIndex;
    
    [HideInInspector] public bool isAdAvailable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        CheckAdblockStatus();
    }

    private void Start()
    {
        CheckCrazyAdAvailability();
    }


    #region CheckAdAvailability

    private void CheckCrazyAdAvailability()
    {
        if (CrazySDK.IsAvailable)
        {
            isAdAvailable = true;
            CrazySDK.Init(() =>
            {
                Debug.Log("CrazySDK initialized");
                // SceneManager.LoadSceneAsync(nextSceneIndex);
            });
        }
        else
        {
            isAdAvailable = false;
            // SceneManager.LoadSceneAsync(nextSceneIndex);
        }
    }

    #endregion

    #region AdBlockStatus

    private void CheckAdblockStatus()
    {
        CrazySDK.Ad.HasAdblock(adblockPresent =>
        {
            Debug.LogWarning("Has adblock: " + adblockPresent);
        });
    }

    private IEnumerator StartAdblockStatusMonitoring()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion

    #region RewardedAds

    public void ShowRewardedAd(Action callback)
    {
        CrazySDK.Ad.RequestAd(
            CrazyAdType.Rewarded,
            () =>
            {
                Debug.Log("Rewarded ad started");
                CrazySDK.Game.GameplayStop(); 
            },
            (error) =>
            {
                Debug.Log("Rewarded ad error: " + error);
                CrazySDK.Game.GameplayStart(); 
            },
            () =>
            {
                Debug.Log("Rewarded ad finished, reward the player here");
                callback?.Invoke();
                CrazySDK.Game.GameplayStart();
            }
        );
    }

    #endregion

    #region BannerAds

    public void ShowBannerAds()
    {
        if (banner != null)
        {
            Instantiate(banner, banner.transform.position, Quaternion.identity);
            CrazySDK.Banner.RefreshBanners();
        }
        else
        {
            Debug.LogWarning("BannerDisplayManager: Banner is not assigned in the inspector.");
        }
    }

    public void HideBannerAds()
    {
        CrazySDK.Banner.Banners.ForEach(b => b.gameObject.SetActive(false));
        CrazySDK.Banner.RefreshBanners();
    }

    #endregion

    #region MidGameAds

    public void ShowMidGameAd()
    {
        CrazySDK.Ad.RequestAd(
            CrazyAdType.Midgame,
            () =>
            {
                Debug.Log("Midgame ad started");
            },
            (error) =>
            {
                Debug.Log("Midgame ad error: " + error);
            },
            () =>
            {
                Debug.Log("Midgame ad finished");
            }
        );
    }

    #endregion
}
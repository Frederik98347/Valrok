using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    static AdsManager instance;
    public static AdsManager Instance { get => instance; }

    public event System.Action OnAdsFinished;
    public event System.Action OnAdsSkipped;
    public event System.Action OnAdsFailed;

    public bool AdPlaying { get; private set; }

    static bool adsDisabled = false;
    public static bool AdsDisabled { get => adsDisabled; }

    int sessionsSinceRewardedAd = 3;
    readonly int sessionsBeforeRewardedAd = 3;

    const string androidID = "3573377";
    const string appleID = "3573376";

    const bool testMode = false;

    System.Action rewardedAdAction;

    /*
    
    private RewardedAd bonusRollRewardedAd;
    private RewardedAd retryBoostRewardedAd;
    private RewardedAd diamondRewardedAd;
    private RewardedAd chestRewardedAd;
    private RewardedAd questRerollRewardedAd;
    private RewardedAd itemBonusStatRerollRewardedAd;

    */

    public enum AdType // admob
    {
        BONUS_ROLL,
        DIAMONDS,
        RETRY_BOOST,
        CHEST,
        QUEST_REROLL,
        ITEM_BONUS_STAT_REROLL
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //init ads
    }

    string GetPlacement(AdType type)
    {
        string ad = "";

        if (!testMode && !Application.isEditor)
        {
            /*
             * Ad placements
             */ 
        }
        else
        {
            // test ads
        }

        return ad;
    }

    public bool CanShowRewardedAd(AdType type)
    {
        if (adsDisabled) return false;

        bool isLoaded = false;

        if (sessionsSinceRewardedAd >= sessionsBeforeRewardedAd)
        {
            isLoaded = IsLoaded(type);
        }

        return isLoaded;
    }

    public bool IsLoaded(AdType type)
    {
        bool isLoaded = false;

        if (type == AdType.RETRY_BOOST)
        {
            //isLoaded = this.retryBoostRewardedAd.IsLoaded();

        }

        return isLoaded;
    }

    void ShowAd(AdType type)
    {
        string p = GetPlacement(type);

        if (type == AdType.BONUS_ROLL)
        {
            /*
            if (this.bonusRollRewardedAd.IsLoaded())
            {
                this.bonusRollRewardedAd.Show();
                sessionsSinceRewardedAd = 0;
            }
           */
        }
    }

    public void ShowRewardedAd(AdType type, System.Action action = null)
    {
        if (adsDisabled) return;

        this.rewardedAdAction = action;

        ShowAd(type);
    }

    public void IncreaseRewardedAdSession()
    {
        sessionsSinceRewardedAd++;
        Debug.Log("Sessions Since Rewarded Ad Increased");
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        AdPlaying = false;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        AdPlaying = true;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        /*
        if (showResult == ShowResult.Failed)
        {
            OnAdsFailed?.Invoke();
        } else if (showResult == ShowResult.Skipped)
        {
            OnAdsSkipped?.Invoke();

            bool rewarded = placementId == "RewardedAd";
            UnityEngine.Analytics.AnalyticsEvent.AdSkip(rewarded);

            if (rewarded)
            {
                rewardedAdAction?.Invoke();
            }
        } else
        {
            OnAdsFinished?.Invoke();

            bool rewarded = placementId == "RewardedAd";
            UnityEngine.Analytics.AnalyticsEvent.AdComplete(rewarded);

            if (rewarded)
            {
                rewardedAdAction?.Invoke();
            }
        }

        AdPlaying = false;

        */
    } // UNITY Ads
}
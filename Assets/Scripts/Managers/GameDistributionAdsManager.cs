using UnityEngine;

public class GameDistributionAdsManager : MonoBehaviour, IGameDistributionAdsHandler
{
    public IGameManager GameHandler { get; set; }
    
    private void Start()
    {
        GameDistribution.OnResumeGame += OnResumeGame;
        GameDistribution.OnPauseGame += OnPauseGame;
        GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        GameDistribution.OnRewardGame += OnRewardGame;
        
        PreloadRewardedAd();
    }

    private void OnResumeGame()
    {
        // Resume the game
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Debug.Log("Game Resumed");
    }

    private void OnPauseGame()
    {
        // Pause the game
        Time.timeScale = 0f;
        AudioListener.pause = true;
        Debug.Log("Game Paused");
    }

    private void OnRewardGame()
    {
        // Reward the player
        GameHandler.LoadGameState();
        Debug.Log("Player Rewarded");
    }

    private void OnRewardedVideoSuccess()
    {
        // Rewarded video succeeded/completed
        Debug.Log("Rewarded Video Success");
    }

    private void OnRewardedVideoFailure()
    {
        // Rewarded video failed
        Debug.Log("Rewarded Video Failed");
    }

    private void OnPreloadRewardedVideo(int loaded)
    {
        Debug.Log("Rewarded Video Preload Status: " + (loaded == 1 ? "Success" : "Failed"));
    }
    
    public void PreloadRewardedAd()
    {
        GameDistribution.Instance.PreloadRewardedAd();
    }

    public void ShowAd()
    {
        GameDistribution.Instance.ShowAd();
    }

    public void ShowRewardedAd()
    {
        if (GameDistribution.Instance.IsRewardedVideoLoaded())
        {
            GameDistribution.Instance.ShowRewardedAd();
        }
        else
        {
            Debug.Log("Rewarded ad is not loaded yet. Preloading...");
            PreloadRewardedAd();
        }
    }
}
using TMPro;
using UnityEngine;

public class Currency: MonoBehaviour, ICurrency
{
    [SerializeField] private TextMeshProUGUI _coinTextInventory;
    [SerializeField] private TextMeshProUGUI _coinTextMain;
    [SerializeField] private TextMeshProUGUI _coinsEarnedText;
    [SerializeField] private Animator _coinAnimator;

    private int _totalCoins;
    private int _levelCoins = 0;
    private int CoinAnimatorKey;
    
    public IAudioManager AudioHandler { get; set; }

    private void Start()
    {
        CoinAnimatorKey = Animator.StringToHash("coin");
        GetSavedCoins();
        UpdateCoinTexts();
    }

    public void AddLevelCoins(int amount)
    {
        _levelCoins += amount;
        SetLevelEarnedCoinsText(_levelCoins);
    }
    
    public int GetLevelCoins()
    {
        return _levelCoins;
    }
    
    public void SetLevelEarnedCoinsText(int amount)
    {
        if (_coinsEarnedText != null && int.Parse(_coinsEarnedText.text) >= 0)
        {
            _coinsEarnedText.text = $"+{amount}";
        } 
    }
    
    public void SaveLevelCoins()
    {
        UpdateCoinTexts();
        SetLevelEarnedCoinsText(_levelCoins);
    }
    
    public void ResetLevelCoins()
    {
        _levelCoins = 0;
        SetLevelEarnedCoinsText(_levelCoins);
    }

    public void AddTotalCoins(int value)
    {
        AudioHandler.PlaySFX("Score");
        _coinAnimator.SetTrigger(CoinAnimatorKey);
        AddCoinsInTotal(value);
        SaveTotalCoins();
        UpdateCoinTexts();
    }
    
    public void AddCoinsInTotal(int amount)
    {
        _totalCoins += amount;
    }
    
    public void SetCoinsOnGameOver()
    {
        AddCoinsInTotal(_levelCoins);
        SetLevelEarnedCoinsText(_levelCoins);
        SaveLevelCoins();
    }
    
    private void GetSavedCoins()
    {
        _totalCoins = PlayerPrefs.GetInt("Coins", 0);
    }

    public int GetTotalCoins()
    {
        return _totalCoins;
    }
    
    private void SaveTotalCoins()
    {
        PlayerPrefs.SetInt("Coins", _totalCoins);
        PlayerPrefs.Save();
    }
    
    private void UpdateCoinTexts()
    {
        _coinTextInventory.SetText(_totalCoins.ToString());
        _coinTextMain.SetText(_totalCoins.ToString());
    }
}
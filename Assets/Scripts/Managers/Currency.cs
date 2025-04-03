using TMPro;
using UnityEngine;

public class Currency: MonoBehaviour, ICurrency
{
    [SerializeField] private TextMeshProUGUI _coinTextInventory;
    [SerializeField] private TextMeshProUGUI _coinTextMain;
    [SerializeField] private Animator _coinAnimator;

    private int _coins;
    private int CoinAnimatorKey;
    
    public IAudioManager AudioHandler { get; set; }

    private void Start()
    {
        _coins = 0;
        CoinAnimatorKey = Animator.StringToHash("coin");
        GetSavedCoins();
    }

    public void Increment(int value)
    {
        AudioHandler.PlaySFX("Score");
        _coinAnimator.SetTrigger(CoinAnimatorKey);
        _coins += value;
        _coinTextInventory.SetText(_coins.ToString());
    }

    public void SetAmount(int value)
    {
        _coins = value;
    }
    
    public void SaveTotalCoins()
    {
        PlayerPrefs.SetInt("Coins", _coins);
        PlayerPrefs.Save();
    }
    
    private void GetSavedCoins()
    {
        _coins = PlayerPrefs.GetInt("Coins", 0);
    }

    public int GetCoins()
    {
        return _coins;
    }
}
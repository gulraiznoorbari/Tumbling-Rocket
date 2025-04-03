using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IInventoryManager
{
    [SerializeField] private RocketDatabase _rocketDatabase;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _notEnoughCoinsText;
    [SerializeField] private Button _selectButton;
    [SerializeField] private TextMeshProUGUI _selectedText;
    [Header("Character Info")]
    [SerializeField] private Image _sprite;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;

    private int _selectedOption = 0;
    private int _equippedRocket = 0;
    private HashSet<int> _ownedRockets = new HashSet<int>();
    
    public ICurrency CurrencyHandler { get; set; }
    public ISaveManager SaveHandler { get; set; }
    public IPlayerHandler PlayerHandler { get; set; }
    
    private void Start()
    {
        // CheckIfLeftButtonScroll();
        // CheckIfRightButtonScroll();
        UpdateRocketData(_selectedOption);
        LoadOwnedRockets();
        LoadEquippedRocket();
        LoadPlayerSprite();
    }

    private void OnEnable()
    {
        _left.onClick.AddListener(Previous);
        _right.onClick.AddListener(Next);
        _buyButton.onClick.AddListener(OnBuyButtonClicked);
        _selectButton.onClick.AddListener(OnSelectedButtonClicked);
    }

    private void OnDisable()
    {
        _left.onClick.RemoveListener(Previous);
        _right.onClick.RemoveListener(Next);
        _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        _selectButton.onClick.RemoveListener(OnSelectedButtonClicked);
    }

    private void UpdateRocketData(int selectedOption)
    {
        var rocket = _rocketDatabase.GetSelectedRocket(selectedOption);
        _sprite.sprite = rocket.rocketSprite;
        _name.text = rocket.rocketName;
        _cost.text = rocket.rocketCost >= 1 ? rocket.rocketCost.ToString() : "";
        var isOwned = _ownedRockets.Contains(selectedOption);
        var isEquipped = _equippedRocket == selectedOption;
        UpdateBuyButtonState(rocket, isOwned, isEquipped);
    }

    private void UpdateBuyButtonState(Rocket rocket, bool isOwned, bool isEquipped)
    {
        _buyButton.gameObject.SetActive(false);
        _selectButton.gameObject.SetActive(false);
        _selectedText.gameObject.SetActive(false);
        _notEnoughCoinsText.gameObject.SetActive(false);
        if (isEquipped)
        {
            _selectedText.gameObject.SetActive(true);
        }
        else if (isOwned)
        {
            _selectButton.gameObject.SetActive(true);
        }
        else
        {
            _buyButton.gameObject.SetActive(true);
            var canAfford = CurrencyHandler.GetTotalCoins() >= rocket.rocketCost;
            _buyButton.interactable = canAfford;
            _notEnoughCoinsText.gameObject.SetActive(rocket.rocketCost > 0 && !canAfford);
        }
        // _buyButton.gameObject.SetActive(!isOwned);
        // if (!isOwned)
        // {
        //     var canAfford = CurrencyHandler.GetCoins() >= rocket.rocketCost;
        //     _buyButton.interactable = canAfford;
        //     _notEnoughCoinsText.gameObject.SetActive(rocket.rocketCost > 0 && !canAfford);
        // }
    }
    
    private void OnBuyButtonClicked()
    {
        var rocket = _rocketDatabase.GetSelectedRocket(_selectedOption);
        if (CurrencyHandler.GetTotalCoins() < rocket.rocketCost)
        {
            ShowNotEnoughCoinsMessage();
            return;
        }
        CurrencyHandler.AddTotalCoins(-rocket.rocketCost);
        _ownedRockets.Add(_selectedOption);
        UpdateRocketData(_selectedOption);
        SaveOwnedRockets();
    }

    private void ShowNotEnoughCoinsMessage()
    {
        _notEnoughCoinsText.gameObject.SetActive(true);
    }

    private void OnSelectedButtonClicked()
    {
        EquipRocket(_selectedOption);
    }

    private void EquipRocket(int selectedOption)
    {
        var previouslyEquipped = _equippedRocket;
        _equippedRocket = selectedOption;
        var rocket = _rocketDatabase.GetSelectedRocket(selectedOption);
        // change player rocket sprite here:
        PlayerHandler.SetPlayerSprite(rocket.rocketSprite);
        if (previouslyEquipped != selectedOption)
        {
            UpdateRocketData(previouslyEquipped); 
            UpdateRocketData(selectedOption);    
        }
        SaveEquippedRocket();
    }

    private void Next()
    {
        _selectedOption++;
        // CheckIfRightButtonScroll();
        if (_selectedOption >= _rocketDatabase.RocketsCount)
        {
            _selectedOption = 0;
        }
        UpdateRocketData(_selectedOption);
    }

    private void Previous()
    {
        _selectedOption--;
        // CheckIfLeftButtonScroll();
        if (_selectedOption < 0)
        {
            _selectedOption = _rocketDatabase.RocketsCount - 1;
        }
        UpdateRocketData(_selectedOption);
    }

    private void CheckIfLeftButtonScroll()
    {
        _left.gameObject.SetActive(_selectedOption <= 0);
    }

    private void CheckIfRightButtonScroll()
    {
        _right.gameObject.SetActive(_selectedOption >= _rocketDatabase.RocketsCount);
    }

    private void LoadPlayerSprite()
    {
        if (_rocketDatabase.RocketsCount > 0)
        {
            PlayerHandler.SetPlayerSprite(_rocketDatabase.GetSelectedRocket(_equippedRocket).rocketSprite);
        }
    }

    private void LoadEquippedRocket()
    {
        var savedState = SaveHandler.GetCurrentGameState();
        if (savedState != null)
        {
            _equippedRocket = savedState.EquippedRocket;
        }
    }
    
    private void LoadOwnedRockets()
    {
        var savedState = SaveHandler.GetCurrentGameState();
        if (savedState != null)
        {
            _ownedRockets = new HashSet<int>(savedState.OwnedRockets);
        }
        
        // Always own the first rocket by default
        if (_rocketDatabase.RocketsCount > 0 && _ownedRockets.Count == 0)
        {
            _ownedRockets.Add(0);
            EquipRocket(0);
            SaveOwnedRockets();
        }
    }

    private void SaveOwnedRockets()
    {
        var currentGameState = SaveHandler.GetCurrentGameState();
        currentGameState.OwnedRockets = _ownedRockets;
        // var currentState = new PlayerSave
        // {
        //     CurrentScore = currentGameState.CurrentScore,
        //     OwnedRockets = _ownedRockets,
        //     EquippedRocket = _equippedRocket,
        //     PipesPositions = currentGameState.PipesPositions,
        //     PlayerPosition = currentGameState.PlayerPosition
        // };
        SaveHandler.UpdateGameState(currentGameState);
        SaveHandler.Save();
    }
    
    private void SaveEquippedRocket()
    {
        var currentGameState = SaveHandler.GetCurrentGameState();
        currentGameState.EquippedRocket = _equippedRocket;
        SaveHandler.UpdateGameState(currentGameState);
        SaveHandler.Save();
    }
}

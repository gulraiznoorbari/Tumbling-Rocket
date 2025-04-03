using UnityEngine;

namespace DependencyInjector
{
    public class DependencyInjector : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private SaveManager _saveManager;
        [SerializeField] private InventoryManager _inventoryManager;
        [SerializeField] private Currency _currency;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PipePool _pipePoolingManager;

        private void Awake()
        {
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            _uiManager.GameHandler = _gameManager;
            _inventoryManager.PlayerHandler = _playerMovement;
            _inventoryManager.CurrencyHandler = _currency;
            _inventoryManager.SaveHandler = _saveManager;
            _currency.AudioHandler = _audioManager;
            _playerMovement.AudioHandler = _audioManager;
            _playerMovement.GameHandler = _gameManager;
            _playerMovement.UIHandler = _uiManager;
            _gameManager.AudioHandler = _audioManager;
            _gameManager.SaveHandler = _saveManager;
            _gameManager.PlayerHandler = _playerMovement;
            _gameManager.PipePoolHandler = _pipePoolingManager;
        }
    }
}
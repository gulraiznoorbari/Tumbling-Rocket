using UnityEngine;

namespace DependencyInjector
{
    public class DependencyInjector : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private SaveManager _saveManager;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PipePool _pipePoolingManager;
        [SerializeField] private GameDistributionAdsManager _gameDistributionAdsManager;

        private void Awake()
        {
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            _uiManager.GameHandler = _gameManager;
            _uiManager.GameDistributionAdsHandler = _gameDistributionAdsManager;
            _playerMovement.AudioHandler = _audioManager;
            _playerMovement.GameHandler = _gameManager;
            _playerMovement.UIHandler = _uiManager;
            _playerMovement.GameDistributionAdsHandler = _gameDistributionAdsManager;
            _gameManager.AudioHandler = _audioManager;
            _gameManager.SaveHandler = _saveManager;
            _gameManager.PlayerHandler = _playerMovement;
            _gameManager.PipePoolHandler = _pipePoolingManager;
            _gameDistributionAdsManager.GameHandler = _gameManager;
        }
    }
}
﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IUIManager
{
	[SerializeField] private ParticleSystem flame;
	[Header("Game State")] 
	[SerializeField] private GameObject _gameStatePanel;
	[SerializeField] private Button _pauseButton;
	[SerializeField] private Button _resumeButton;
	[SerializeField] private GameObject _background;
	[Header("Game Over Menu")] 
	[SerializeField] private GameObject _gameOverMenuPanel;
	[SerializeField] private Button _mainMenuButton;
	[SerializeField] private Button _retryButton;
	[Header("Main Menu")]
	[SerializeField] private GameObject _mainMenu;
	[SerializeField] private GameObject _settingsMenu;
	[SerializeField] private Button _playButton;
	[SerializeField] private Button _inventoryButton;
	[SerializeField] private Button _settingsButton;
	[Header("Inventory")] 
	[SerializeField] private GameObject _inventoryMenuPanel;
	
	private ParticleSystem.EmissionModule flameEmission;
	public IGameManager GameHandler { get; set; }

	private void Awake()
	{
		flameEmission = flame.emission;
	}

	private void OnEnable()
	{
		_playButton.onClick.AddListener(PlayGame);
		_settingsButton.onClick.AddListener(EnableSettingsMenu);
		_inventoryButton.onClick.AddListener(EnableInventoryMenu);
		_mainMenuButton.onClick.AddListener(EnableMainMenu);
		_retryButton.onClick.AddListener(RetryGame);
		_pauseButton.onClick.AddListener(EnablePause);
		_resumeButton.onClick.AddListener(EnableResume);
	}

	private void OnDisable()
	{
		_playButton.onClick.RemoveListener(PlayGame);
		_settingsButton.onClick.RemoveListener(EnableSettingsMenu);
		_inventoryButton.onClick.RemoveListener(EnableInventoryMenu);
		_mainMenuButton.onClick.RemoveListener(EnableMainMenu);
		_retryButton.onClick.RemoveListener(RetryGame);
		_pauseButton.onClick.RemoveListener(EnablePause);
		_resumeButton.onClick.RemoveListener(EnableResume);
	}

	private void PlayGame()
	{
		GameHandler.SetGameOver(false);
		_mainMenu.SetActive(false);
		flameEmission.enabled = false;
		GameHandler.ResumeGame();
	}

	private void EnableSettingsMenu()
	{
		_mainMenu.SetActive(false);
		_gameStatePanel.SetActive(false);
		_settingsMenu.SetActive(true);
	}

	private void RetryGame()
	{
		GameHandler.SetGameOver(false);
		_mainMenu.SetActive(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		_gameStatePanel.SetActive(true);
		Time.timeScale = 1f;
	}

	private void EnableMainMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 0f;
		_mainMenu.SetActive(true);
		_gameStatePanel.SetActive(false);
	}

	private void EnablePause()
	{
		GameHandler.PauseGame();
		_pauseButton.gameObject.SetActive(false);
		_resumeButton.gameObject.SetActive(true);
		_background.SetActive(true);
	}

	private void EnableResume()
	{
		GameHandler.ResumeGame();
		_pauseButton.gameObject.SetActive(true);
		_resumeButton.gameObject.SetActive(false);
		_background.SetActive(false);
	}

	public void EnableGameOverMenu()
	{
		GameHandler.GetScore();
		_gameOverMenuPanel.SetActive(true);
		_gameStatePanel.SetActive(false);
	}

	private void EnableInventoryMenu()
	{
		_inventoryMenuPanel.SetActive(true);
	}
}

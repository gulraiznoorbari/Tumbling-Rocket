using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	private bool _playerDead;

	[SerializeField] private GameObject _mainMenu;
	[SerializeField] private ParticleSystem flame;
	private ParticleSystem.EmissionModule flameEmission;

	private void Awake()
	{
		_playerDead = FindObjectOfType<PlayerMovement>().dead;
		flameEmission = flame.emission;
	}

	public void PlayGame()
	{
		_playerDead = false;
		_mainMenu.SetActive(false);
		flameEmission.enabled = false;
		Time.timeScale = 1f;
	}

	public void RetryGame()
	{
		_playerDead = false;
		_mainMenu.SetActive(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 0;
		_mainMenu.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	private bool _playerDead;
	private Vector2 _playerStartPosition;

	[SerializeField] private GameObject _mainMenu;

	private void Awake()
	{
		_playerDead = FindObjectOfType<PlayerMovement>().dead;
		_playerStartPosition = FindObjectOfType<PlayerMovement>()._startPos;
	}

	public void PlayGame()
	{
		_playerDead = false;
		_mainMenu.SetActive(false);
		Time.timeScale = 1f;
	}

	public void RetryGame()
	{
		//transform.position = _playerStartPosition;
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

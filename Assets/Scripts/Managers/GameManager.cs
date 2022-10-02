using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	private GameObject _startPipe;
	private PlayerSave data;
	private float _timer = 0;
	private int _score;
	private static int _highScore;

	[SerializeField] private GameObject _pipe;
	[SerializeField] private SaveManager _saveManager;
	[SerializeField] private TextMeshProUGUI _scoreText;
	[SerializeField] private TextMeshProUGUI _levelScoreText;
	[SerializeField] private TextMeshProUGUI _highScoreText;
	[SerializeField] private float _startOffset;
	[SerializeField] private float _maxTime = 1f;
	[SerializeField] private float _minheight;
	[SerializeField] private float _maxheight;
	[SerializeField] private Animator _scoreAnimator;

	private int ScoreAnimatorKey;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		ScoreAnimatorKey = Animator.StringToHash("score");
		_score = 0;
		_scoreText.SetText(_score.ToString());
		Time.timeScale = 0;
	}

	private void Start()
	{
		FindObjectOfType<AudioManager>().PlayBGSong();
		transform.position = new Vector3(transform.position.x + 1 , transform.position.y, 0);
		_startPipe = Instantiate(_pipe, transform.position, Quaternion.identity);
	}

	private void Update()
	{
		SpawnPipe();
	}

	public void SpawnPipe()
	{
		if (_timer > _maxTime)
		{
			transform.position = new Vector3(transform.position.x + _startOffset, Random.Range(_minheight, _maxheight), 0);
			_startPipe = Instantiate(_pipe, transform.position, Quaternion.identity);

			_timer = 0;
		}
		_timer += Time.deltaTime;
	}

	public void IncreaseScore()
	{
		FindObjectOfType<AudioManager>().PlaySFX("Score");
		_scoreAnimator.SetTrigger(ScoreAnimatorKey);
		_score++;
		_scoreText.SetText(_score.ToString());
	}

	public void GetScore()
	{
		_levelScoreText.SetText(_score.ToString());

		if (_score > _highScore)
		{
			_highScore = _score;
			_highScoreText.SetText(_highScore.ToString());
			_saveManager.Save();
		}
		if (_score <= _highScore)
		{
			_highScoreText.SetText(_highScore.ToString());
			_saveManager.Save();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1f;
	}
}

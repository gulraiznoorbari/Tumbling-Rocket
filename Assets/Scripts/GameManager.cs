using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	private GameObject _startPipe;
	private Animator _animator;
	private float _timer = 0;
	private int _score;

	[SerializeField] private GameObject _pipe;
	[SerializeField] private TextMeshProUGUI _scoreText;
	[SerializeField] private float _startOffset;
	[SerializeField] private float _maxTime = 1f;
	[SerializeField] private float _minheight;
	[SerializeField] private float _maxheight;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		_animator = FindObjectOfType<Animator>();
		_score = 0;
		_scoreText.SetText(_score.ToString());
	}

	private void Start()
	{
		transform.position = new Vector3(transform.position.x , transform.position.y, 0);
		_startPipe = Instantiate(_pipe, transform.position, Quaternion.identity);
	}

	private void Update()
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
		_animator.SetTrigger("score");
		_score++;
		_scoreText.SetText(_score.ToString());
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

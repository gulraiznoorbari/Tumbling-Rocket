using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IGameManager
{
    private bool _isToTheLeftOfPlayer;
    private bool _isGameOver = false;
    private int _score;
    private int _highScore;
    private int ScoreAnimatorKey;
    private float _timer = 0f;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _levelScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private float _startOffset;
    [SerializeField] private float _maxTime = 1f;
    [SerializeField] private float _minheight;
    [SerializeField] private float _maxheight;
    [SerializeField] private Animator _scoreAnimator;
    
    public IAudioManager AudioHandler { get; set; }
    public ISaveManager SaveHandler { get; set; }
    public IPlayerHandler PlayerHandler { get; set; }
    public IPipePoolHandler PipePoolHandler { get; set; }

    public bool IsGameOver() => _isGameOver;

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;
        ScoreAnimatorKey = Animator.StringToHash("score");
        _score = 0;
        Time.timeScale = 0f;
        _scoreText.SetText(_score.ToString());
    }

    private void Start()
    {
        GetHighScore();
        AudioHandler.PlayBGSong();
    }

    private void Update()
    {
        if (!_isGameOver)
        {
            SpawnPipe();
        }
    }

    private void SpawnPipe()
    {
        if (_timer > _maxTime)
        {
            var pipe = PipePoolHandler.GetPipe();
            if (pipe != null)
            {
                pipe.transform.position = new Vector3(transform.position.x + _startOffset, Random.Range(_minheight, _maxheight), 0);
                pipe.gameObject.SetActive(true);
            }

            _timer = 0;
        }
        _timer += Time.deltaTime;
    }

    public void IncreaseScore()
    {
        if (_isGameOver) return;

        var mostRecentPipe = PipePoolHandler.GetMostRecentPipe();
        if (mostRecentPipe != null)
        {
            _isToTheLeftOfPlayer = mostRecentPipe.transform.position.x > _playerTransform.position.x;
            if (_isToTheLeftOfPlayer)
            {
                AudioHandler.PlaySFX("Score");
                _scoreAnimator.SetTrigger(ScoreAnimatorKey);
                _score++;
                _scoreText.SetText(_score.ToString());
            }
        }
    }

    public void GetScore()
    {
        _levelScoreText.SetText(_score.ToString());

        if (_score > _highScore)
        {
            _highScore = _score;
            _highScoreText.SetText(_highScore.ToString());
            SaveHighScore();
        }
        else
        {
            _highScoreText.SetText(_highScore.ToString());
        }
    }

    // Call on game fail
    public void SaveGameState()
    {
        var currentState = new PlayerSave
        {
            CurrentScore = _score,
            PlayerPosition = PlayerHandler.GetPlayerPosition().position,
            PipesPositions = GetPipesPositions()
        };
        SaveHandler.UpdateGameState(currentState);
        SaveHandler.Save();
    }

    private List<Vector2> GetPipesPositions()
    {
        var pipePositions = new List<Vector2>();
        var pipes = PipePoolHandler.GetAllPipes();
        foreach (var pipe in pipes)
        {
            pipePositions.Add(pipe.transform.position);
        }

        return pipePositions;
    }

    // For Resuming Game (on game fail) on the basis of rewarded ad: 
    public void LoadGameState()
    {
        var savedState = SaveHandler.GetCurrentGameState();
        _score = savedState.CurrentScore;
        PlayerHandler.GetPlayerPosition().position = savedState.PlayerPosition;
        SpawnPipesAtPositions(savedState.PipesPositions);
    }

    private void SpawnPipesAtPositions(List<Vector2> savedPipesPositions)
    {
        foreach (var position in savedPipesPositions)
        {
            var pipe = PipePoolHandler.GetPipe();
            pipe.transform.position = position;
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", _highScore);
        PlayerPrefs.Save();
    }

    private void GetHighScore()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
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
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerHandler
{
    private Touch _touch;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private float _gravity;
    private bool _alreadyPlayed = false;
    private ParticleSystem.EmissionModule _smokeEmission;
    private ParticleSystem.EmissionModule _flameEmission;
    
    [SerializeField] private GameObject _playerNose;
    [Header("Particles")]
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _flame;
    [SerializeField] private ParticleSystem _death;
    
    public IAudioManager AudioHandler { get; set; }
    public IGameManager GameHandler { get; set; }
    public IUIManager UIHandler { get; set; }
    public ICurrency CurrencyHandler { get; set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gravity = _rigidbody.gravityScale;
        _smokeEmission = _smoke.emission;
        _flameEmission = _flame.emission;
    }

    private void FixedUpdate()
    {
        if (GameHandler.IsGameOver()) return;

        // Rotation towards velocity direction:
        var velocity = _rigidbody.velocity;
        var angle = Mathf.Atan2(velocity.y, 10) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 88));

        // Touch Controls:
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began || _touch.phase == TouchPhase.Stationary)
            {
                AudioHandler.PlaySFX("Fuel");
                _flameEmission.enabled = true;
                _rigidbody.AddForce(Vector2.up * (_gravity * Time.deltaTime * 1200f));
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                AudioHandler.StopSFX("Fuel");
                _flameEmission.enabled = false;
            }
        }

        // Keyboard Controls:
        if (Input.GetKey(KeyCode.Space))
        {
            AudioHandler.PlaySFX("Fuel");
            _flameEmission.enabled = true;
            _rigidbody.AddForce(Vector2.up * (_gravity * Time.deltaTime * 1200f));
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            AudioHandler.StopSFX("Fuel");
            _flameEmission.enabled = false;
        }
    }

    private void DeathEffect()
    {
        if (_playerNose != null)
        {
            Instantiate(_death, _playerNose.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameHandler.IsGameOver()) return;

        if (collision.CompareTag("ScoreDetector"))
        {
            GameHandler.IncreaseScore();
        }

        if (collision.CompareTag("Coin"))
        {
            CurrencyHandler.AddLevelCoins(1);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (GameHandler.IsGameOver()) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
        {
            GameHandler.SetGameOver(true);
            AudioHandler.StopSFX("Fuel");
            DeathEffect();
            if (!_alreadyPlayed)
            {
                AudioHandler.PlaySFX("Dead");
                _alreadyPlayed = true;
            }
            _smokeEmission.enabled = false;
            _flameEmission.enabled = false;
            _rigidbody.AddForce(Vector2.down * 130f);
            _rigidbody.AddTorque(30f);
            StartCoroutine(FreezeTime());
        }
    }

    private IEnumerator FreezeTime()
    {
        yield return new WaitForSeconds(1.5f);
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.freezeRotation = true;
        Time.timeScale = 0f;
        GameHandler.SaveGameState();
        CurrencyHandler.SetCoinsOnGameOver();
        UIHandler.EnableGameOverMenu();
    }

    public Transform GetPlayerPosition()
    {
        return gameObject.transform;
    }

    public Sprite GetPlayerCurrentSprite()
    {
        return _spriteRenderer.sprite;
    }

    public void SetPlayerSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
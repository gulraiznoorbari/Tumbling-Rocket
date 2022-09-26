using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Touch _touch;
	private Rigidbody2D _rigidbody;
	private float gravity;
	[HideInInspector] public Vector2 _startPos;
	[HideInInspector] public bool dead;

	[SerializeField] private GameObject _gameOverMenu;
	public static PlayerMovement instance;

	private void Awake()
	{
		Time.timeScale = 0;
		instance = this;
		_rigidbody = GetComponent<Rigidbody2D>();
		_startPos = transform.position;
		gravity = _rigidbody.gravityScale;
	}

	private void Update()
	{
		if (dead) return;

		// rotation towards velocity direction:
		Vector2 velocity = _rigidbody.velocity;
		float angle = Mathf.Atan2(velocity.y, 10) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

		if(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
		{
			_rigidbody.AddForce(Vector2.up * gravity * Time.deltaTime * 1400f);
		}

		if (Input.touchCount > 0)
		{
			_touch = Input.GetTouch(0);
			if (_touch.phase == TouchPhase.Began)
			{
				_rigidbody.AddForce(Vector2.up * gravity * Time.deltaTime * 1400f);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("ScoreDetector"))
		{
			FindObjectOfType<GameManager>().IncreaseScore();
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
		{
			dead = true;
			_rigidbody.AddForce(Vector2.down * 150f);
			_rigidbody.AddTorque(30f);
			if (other.gameObject.CompareTag("Lower Obstacle") || other.gameObject.CompareTag("Ground"))
			{
				_rigidbody.freezeRotation = true;
				StartCoroutine(FreezeTime());
			}
		}
	}
	private IEnumerator FreezeTime()
	{
		yield return new WaitForSeconds(1f);
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.freezeRotation = false;
		Time.timeScale = 0;
		dead = false;
		FindObjectOfType<GameManager>().GetScore();
		_gameOverMenu.SetActive(true);
	}
}

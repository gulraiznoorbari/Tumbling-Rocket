using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	private Touch _touch;
	private Rigidbody2D _rigidbody;
	private Vector2 _startPos;
	private float gravity;
	private bool dead;

	public static PlayerMovement instance;

	private void Start()
	{
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
			_rigidbody.AddForce(Vector2.up * gravity * Time.deltaTime * 1700f);
		}

		if (Input.touchCount > 0)
		{
			_touch = Input.GetTouch(0);
			if (_touch.phase == TouchPhase.Began)
			{
				_rigidbody.AddForce(Vector2.up * gravity * Time.deltaTime * 1700f);
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
			Handheld.Vibrate();
			_rigidbody.AddForce(Vector2.down * 200f);
			_rigidbody.AddTorque(30f);
			if (other.gameObject.CompareTag("Lower Obstacle") || other.gameObject.CompareTag("Ground"))
			{
				_rigidbody.freezeRotation = true;
				StartCoroutine(freezeTime());
			}
		}
	}
	private IEnumerator freezeTime()
	{
		yield return new WaitForSeconds(1f);
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.freezeRotation = false;
		Time.timeScale = 0;
		StartCoroutine(Restart());
	}

	private IEnumerator Restart()
	{
		dead = false;
		yield return new WaitForSeconds(0.05f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1f;
	}
}

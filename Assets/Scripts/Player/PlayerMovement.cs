using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Touch _touch;
	private Rigidbody2D _rigidbody;
	[HideInInspector] public bool dead;
	private float gravity;
	[SerializeField] private GameObject _gameOverMenu;

	[SerializeField] private ParticleSystem ps;
	private ParticleSystem.EmissionModule emission;
	[SerializeField] private ParticleSystem flame;
	private ParticleSystem.EmissionModule flameEmission;

	public static PlayerMovement instance;

	private void Awake()
	{
		instance = this;
		_rigidbody = GetComponent<Rigidbody2D>();
		gravity = _rigidbody.gravityScale;
		emission = ps.emission;
		flameEmission = flame.emission;
		Time.timeScale = 0;
	}

	private void Update()
	{
		if (dead) return;

		// rotation towards velocity direction:
		Vector2 velocity = _rigidbody.velocity;
		float angle = Mathf.Atan2(velocity.y, 10) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 88));

		if(Input.GetKey(KeyCode.Space))
		{
			FindObjectOfType<AudioManager>().PlaySFX("Fuel");
			flameEmission.enabled = true;
			_rigidbody.AddForce(Vector2.up * gravity * Time.deltaTime * 1300f);
		} else if (Input.GetKeyUp(KeyCode.Space))
		{
			FindObjectOfType<AudioManager>().StopSFX("Fuel");
			flameEmission.enabled = false;
		}

		if (Input.touchCount > 0)
		{
			_touch = Input.GetTouch(0);
			if (_touch.phase == TouchPhase.Began)
			{
				FindObjectOfType<AudioManager>().PlaySFX("Fuel");
				flameEmission.enabled = true;
				_rigidbody.AddForce(Vector2.up * gravity * Time.deltaTime * 1300f);
			} else if (_touch.phase == TouchPhase.Ended)
			{
				FindObjectOfType<AudioManager>().StopSFX("Fuel");
				flameEmission.enabled = false;
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
			emission.enabled = false;
			_rigidbody.AddForce(Vector2.down * 150f);
			_rigidbody.AddTorque(30f);
			if (other.gameObject.CompareTag("Lower Obstacle") || other.gameObject.CompareTag("Ground"))
			{
				FindObjectOfType<AudioManager>().PlaySFX("Dead");
				emission.enabled = false;
				flameEmission.enabled = false;
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

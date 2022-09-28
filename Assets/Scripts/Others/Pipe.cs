using UnityEngine;

public class Pipe : MonoBehaviour
{
	[SerializeField] private float _speed;
	private float leftEdge;

	private void Start()
	{
		leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 8f;
	}

	// Update is called once per frame
	private void Update()
    {
		transform.position += Vector3.left * _speed * Time.deltaTime;

		if (transform.position.x < leftEdge)
		{
			Destroy(gameObject);
		}
	}
}

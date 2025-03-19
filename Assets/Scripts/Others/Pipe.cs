using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private float _leftEdge;
    private PipePool _pipePool;

    private void Start()
    {
        _leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 8f;
    }

    private void Update()
    {
        if (!_pipePool) return; 
        
        transform.position += Vector3.left * (_speed * Time.deltaTime);
        if (transform.position.x < _leftEdge)
        {
            _pipePool.ReturnPipe(this);
        }
    }
    
    public void SetPool(PipePool pool)
    {
        _pipePool = pool;
    }
    
}
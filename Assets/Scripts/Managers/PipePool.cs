using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipePool : MonoBehaviour, IPipePoolHandler
{
    [SerializeField] private Pipe _pipePrefab; 
    [SerializeField] private int _poolSize = 5; 
    
    private readonly Queue<Pipe> _pipePool = new Queue<Pipe>();
    private Pipe _mostRecentPipe;

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var pipe = Instantiate(_pipePrefab, transform);
            pipe.gameObject.SetActive(false);
            _pipePool.Enqueue(pipe);
        }
    }

    public List<Pipe> GetAllPipes()
    {
        return _pipePool.ToList();
    }

    public Pipe GetPipe()
    {
        if (_pipePool.Count > 0)
        {
            var pipe = _pipePool.Dequeue();
            pipe.gameObject.SetActive(true);
            _mostRecentPipe = pipe;
            return pipe;
        }
        else
        {
            var pipe = Instantiate(_pipePrefab, transform);
            _mostRecentPipe = pipe;
            return pipe;
        }
    }

    public void ReturnPipe(Pipe pipe)
    {
        pipe.gameObject.SetActive(false); 
        _pipePool.Enqueue(pipe);
    }

    public Pipe GetMostRecentPipe()
    {
        return _mostRecentPipe;
    }
}

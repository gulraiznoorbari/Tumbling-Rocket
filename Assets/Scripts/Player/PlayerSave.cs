using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSave
{
    private int _currentScore;
    private Vector2 _playerPosition;
    private List<Vector2> _pipesPositions;
    private HashSet<int> _ownedRockets;
    private int _equippedRocket;

    public PlayerSave()
    {
        _currentScore = 0;
        _playerPosition = Vector2.zero;
        _pipesPositions = new List<Vector2>();
        _ownedRockets = new HashSet<int>();
        _equippedRocket = 0;
    }

    public int CurrentScore
    {
        get => _currentScore;
        set => _currentScore = value;
    }

    public Vector2 PlayerPosition
    {
        get => _playerPosition;
        set => _playerPosition = value;
    }

    public List<Vector2> PipesPositions
    {
        get => _pipesPositions;
        set => _pipesPositions = value ?? new List<Vector2>();
    }

    public HashSet<int> OwnedRockets
    {
        get => _ownedRockets;
        set => _ownedRockets = value ?? new HashSet<int>();
    }

    public int EquippedRocket
    {
        get => _equippedRocket;
        set => _equippedRocket = value;
    }
}
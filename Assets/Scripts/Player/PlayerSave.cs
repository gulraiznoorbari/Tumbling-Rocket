using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSave
{
    private int currentScore;
    private Vector2 playerPosition;
    private List<Vector2> pipesPositions;

    public PlayerSave()
    {
        currentScore = 0;
        playerPosition = Vector2.zero;
        pipesPositions = new List<Vector2>();
    }

    public int CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }

    public Vector2 PlayerPosition
    {
        get => playerPosition;
        set => playerPosition = value;
    }

    public List<Vector2> PipesPositions
    {
        get => pipesPositions;
        set => pipesPositions = value ?? new List<Vector2>();
    }
}
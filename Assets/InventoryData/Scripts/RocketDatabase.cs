using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Rockets", fileName = "Rockets Data", order = 0)]
public class RocketDatabase : ScriptableObject
{
    [SerializeField] private List<Rocket> _rocketsData;

    public int RocketsCount => _rocketsData.Count;

    public Rocket GetSelectedRocket(int index)
    {
        return _rocketsData[index];
    }
}

[Serializable]
public class Rocket
{
    public string rocketName;
    public Sprite rocketSprite;
    public int rocketCost;
}
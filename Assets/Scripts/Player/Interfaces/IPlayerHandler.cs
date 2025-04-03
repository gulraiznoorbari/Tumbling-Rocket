using UnityEngine;

public interface IPlayerHandler
{
    public Transform GetPlayerPosition();
    public Sprite GetPlayerCurrentSprite();
    public void SetPlayerSprite(Sprite sprite);
}
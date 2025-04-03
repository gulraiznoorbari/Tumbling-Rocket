public interface ICurrency
{
    public void AddLevelCoins(int amount);
    public int GetLevelCoins();
    public void ResetLevelCoins();
    public void AddTotalCoins(int value);
    public int GetTotalCoins();
    public void SetCoinsOnGameOver();
}
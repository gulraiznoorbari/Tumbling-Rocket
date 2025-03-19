public interface IGameManager
{
    public void IncreaseScore();
    public void GetScore();
    public bool IsGameOver();
    public void SetGameOver(bool isGameOver);
    public void PauseGame();
    public void ResumeGame();
    public void SaveGameState();
    public void LoadGameState();
}

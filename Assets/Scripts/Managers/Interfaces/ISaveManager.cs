public interface ISaveManager
{
    public void Save();
    public void Load();
    public void UpdateGameState(PlayerSave newState);
    public PlayerSave GetCurrentGameState();
}
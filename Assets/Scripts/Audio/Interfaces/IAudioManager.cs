public interface IAudioManager
{
    public void PlaySFX(string audioName);
    public void PlayBGSong();
    public void StopSFX(string audioName);
    public void StopBGSong();
}
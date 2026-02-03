using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public MusicManager musicManager;
    public AudioClip musica;

    public void PlayMusic()
    {
        musicManager.TocarMusica(musica);
    }
}

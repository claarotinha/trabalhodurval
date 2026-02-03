using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void TocarMusica(AudioClip musica)
    {
        if (audioSource.clip == musica) return;

        audioSource.Stop();
        audioSource.clip = musica;
        audioSource.Play();
    }
}

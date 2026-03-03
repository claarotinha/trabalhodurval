using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 1.5f;

    public void ChangeMusic(AudioClip newMusic)
    {
        if (audioSource.clip == newMusic) return;
        StartCoroutine(FadeMusic(newMusic));
    }

    IEnumerator FadeMusic(AudioClip newMusic)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.clip = newMusic;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}

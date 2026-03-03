using UnityEngine;

public class StingerManager : MonoBehaviour
{
    public AudioSource stingerSource;

    public void PlayStinger(AudioClip clip)
    {
        if (clip == null) return;
        stingerSource.PlayOneShot(clip);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class SomAoPassarMouse : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip somHover;
    private AudioSource audioSource;

    void Awake()
    {
        // Tenta achar um AudioSource no próprio objeto ou cria um novo
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (somHover != null)
        {
            audioSource.PlayOneShot(somHover);
        }
    }
}

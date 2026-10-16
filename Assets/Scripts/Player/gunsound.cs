using UnityEngine;

public class gunsound : MonoBehaviour
{
    public AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}

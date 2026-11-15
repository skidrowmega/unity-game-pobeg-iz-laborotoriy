using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    //Это класс чтобы оставлялись звуки чего-то в каком-то определенном месте вызываемый из любого скрипта (очень описательно)

    [SerializeField] GameObject EmptyPrefab;

    public static AudioPlayer instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(Vector3 position,AudioClip sound, float Volume=1)
    {
        AudioSource source=Instantiate(EmptyPrefab, position, Quaternion.identity).GetComponent<AudioSource>();
        source.PlayOneShot(sound,Volume);
        Destroy(source.gameObject, sound.length);

    }


}

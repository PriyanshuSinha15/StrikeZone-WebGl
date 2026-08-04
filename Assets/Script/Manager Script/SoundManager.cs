using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public MyAudioClip_SO myAudioClip_SO;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(int clipIndex)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioClip targetClip = myAudioClip_SO.clip[clipIndex];
        audioSource.PlayOneShot(targetClip);

        Destroy(soundGameObject, 3f);
    }
}

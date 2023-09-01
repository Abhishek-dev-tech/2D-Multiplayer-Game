using UnityEngine;

[System.Serializable]
public struct SoundEffects
{
    public string name;

    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource sfxSource;

    [SerializeField]
    private SoundEffects[] soundEffects;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        DontDestroyOnLoad(gameObject);

        sfxSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(string name, float volume = 1f)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].name == name)
                sfxSource.PlayOneShot(soundEffects[i].audioClip, volume);
        }
    }
}

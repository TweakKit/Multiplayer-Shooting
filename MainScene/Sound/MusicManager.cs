using UnityEngine; 

public class MusicManager : MonoBehaviour  {

    public static MusicManager Instance;

    // the songs we use for the corresponding sounds
    public AudioClip shoot; 

    public AudioSource BGSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    { 
        if (BGSource.isPlaying)
            BGSource.Stop();
    }


    private void turnOnSound(AudioClip clip, float volumn, float pitch)
    {
        BGSource.pitch = pitch;
        BGSource.PlayOneShot(clip, volumn);
    }

    public void playSound(AudioClip clip, float volumn, float pitch)
    { 
         turnOnSound(clip, volumn, pitch);
    }
}

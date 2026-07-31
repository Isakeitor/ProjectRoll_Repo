using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip jump;
    public AudioClip enemyShoot;
    public AudioClip bonus;
    public AudioClip pickup;
    public AudioClip portal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jump);
    }

    public void PlayEnemyShoot()
    {
        audioSource.PlayOneShot(enemyShoot);
    }

    public void PlayBonus()
    {
        audioSource.PlayOneShot(bonus);
    }

    public void PlayPickup()
    {
        audioSource.PlayOneShot(pickup);
    }

    public void PlayPortal()
    {
        audioSource.PlayOneShot(portal);
    }
}
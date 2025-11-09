using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource audioSourceTheme;
    public AudioSource audioSourceSFXJump;
    public AudioSource audioSourceSFXButton;
    public AudioSource audioSourceSFXPoint;
    public AudioSource audioSourceSFXShield;
    public AudioSource audioSourceSFXShieldImpact;

    [Header("Music Clips")]
    public AudioClip menuTheme;
    public AudioClip gameTheme;
    public AudioClip gameOverTheme;



    private void Awake()
    {
        audioSourceTheme.clip = menuTheme;
        audioSourceTheme.Play();
    }

    private void Update()
    {
        if (GameController.instance.canPlay)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                audioSourceSFXJump.Stop();
                audioSourceSFXJump.Play();
            }
        }
    }

    public void PointSFX()
    {
        audioSourceSFXPoint.Stop();
        audioSourceSFXPoint.Play();
    }

    public void ButtonSFX()
    {
        audioSourceSFXButton.Stop();
        audioSourceSFXButton.Play();
    }

    public void GameThemeMusic()
    {
        audioSourceTheme.Stop();
        audioSourceTheme.clip = gameTheme;
        audioSourceTheme.Play();
    }

    public void GameOverMusic()
    {
        audioSourceTheme.Stop();
        audioSourceTheme.clip = gameOverTheme;
        audioSourceTheme.Play();
    }

    public void StopAllSFX()
    {
        audioSourceSFXJump.Stop();
        audioSourceSFXPoint.Stop();
        audioSourceSFXShield.Stop();
        audioSourceSFXShieldImpact.Stop();
    }

    public void PlayShieldPickupSFX()
    {
        audioSourceSFXShield.Stop();
        audioSourceSFXShield.Play();
    }

    public void PlayShieldImpactSFX()
    {
        audioSourceSFXShieldImpact.Stop();
        audioSourceSFXShieldImpact.Play();
    }
}
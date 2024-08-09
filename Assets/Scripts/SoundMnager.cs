using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMnager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip btnClick;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip hitClip;
    public AudioClip startLevelClip;
    public Slider audioSlider;

    public static SoundMnager soundManager;

    private void Awake()
    {
        soundManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>(true);
        audioSlider = FindObjectOfType<Slider>(true);
        audioSource.volume = audioSlider.value;
        audioSlider.onValueChanged.AddListener(OnVolumeChange);
    }

    public void PlayBtnClickSound()
    {

        audioSource.PlayOneShot(btnClick);

    }
    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winClip);
    }
    public void PlayLoseSound()
    {

        audioSource.PlayOneShot(loseClip);

    }
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitClip);
    }
    public void PlayStartLevelSound()
    {

        audioSource.PlayOneShot(startLevelClip);

    }

    public void OnVolumeChange(float volume) 
    {
        audioSource.volume = audioSlider.value;
    }

}

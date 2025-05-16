using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfxSource;
    public AudioSource sfxRandomPitchSource;
    public AudioSource musicSource;
    public AudioSource spatialSource1;
    public AudioSource spatialSource2;
    public AudioSource spatialSource3;

    [Header("Settings")]
    public float defaultMusicVolume;
    public float defaultSfxVolume;
    // public bool isMusicMuted;
    // public bool isSfxMuted;

    [Header("Single Sounds")]
    public AudioClip useDoor;
    public AudioClip quizEnd;
    public AudioClip quizStart;
    public AudioClip uiClick;
    public AudioClip selectLevel;
    public AudioClip deselectLevel;
    public AudioClip startLevel;

    [Header("NPC")]
    public AudioClip dialogueStart;
    public AudioClip dialogueProgress;
    public AudioClip dialogueEnd;

    [Header("Player")]
    public AudioClip playerHit;
    public AudioClip playerSword;
    public AudioClip playerDie;

    [Header("Zombies")]
    public AudioClip zombieDetect;
    public AudioClip zombieHit;
    public AudioClip zombieDie;

    [Header("Music")]
    public AudioClip mainMenuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;
    public AudioClip level4Music;
    public AudioClip quizMusic;

    [Header("Sound Lists")]
    public AudioClip[] coinSounds;
    public AudioClip[] swordSounds;
    public AudioClip[] answerClickSounds;
    public AudioClip[] popSounds;
    public AudioClip[] jumpSounds;


    // [Header("References")]
    // public Image musicMute;
    // public Sprite musicMutedSprite;
    // public Sprite musicUnmutedSprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        sfxSource.volume = defaultSfxVolume;
        sfxRandomPitchSource.volume = defaultSfxVolume;
        musicSource.volume = defaultMusicVolume;
        // SetMuteStatus();

        PlayMusic(mainMenuMusic);
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySfx(AudioClip clip, AudioSource audioSource)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlaySfxRandomPitch(AudioClip clip)
    {
        sfxRandomPitchSource.pitch = Random.Range(0.6f, 1.1f);
        sfxRandomPitchSource.PlayOneShot(clip);
    }

    public void PlaySfxRandomPitch(AudioClip clip, AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.6f, 1.1f);
        audioSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySpatial(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomFromList(AudioClip[] list)
    {
        if (list.Length > 0)
        {
            int rand = Random.Range(0, list.Length);
            PlaySfx(list[rand]);
        }
    }

    public void PlayRandomFromList(AudioClip[] list, AudioSource audioSource)
    {
        if (list.Length > 0)
        {
            int rand = Random.Range(0, list.Length);
            PlaySfx(list[rand], audioSource);
        }
    }

    public void PlayRandomFromListRandomPitch(AudioClip[] list)
    {
        if (list.Length > 0)
        {
            int rand = Random.Range(0, list.Length);
            PlaySfxRandomPitch(list[rand]);
        }
    }

    public void PlayRandomFromListRandomPitch(AudioClip[] list, AudioSource audioSource)
    {
        if (list.Length > 0)
        {
            int rand = Random.Range(0, list.Length);
            PlaySfxRandomPitch(list[rand], audioSource);
        }
    }

    public void ChangeMusic(AudioClip clip)
    {
        StartCoroutine(ChangeMusicRoutine(clip));
    }

    private IEnumerator ChangeMusicRoutine(AudioClip clip)
    {
        if (musicSource.isPlaying)
        {
            if (musicSource.clip == clip) yield break;
            FadeMusicOut(0.5f);
            yield return new WaitForSeconds(0.5f);
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
            FadeMusicIn(0.5f);
        }
        else
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void FadeMusicOut(float time)
    {
        if (!DOTween.IsTweening(musicSource))
        {
            musicSource.DOFade(0, time);
        }
        else
        {
            musicSource.DOComplete();
        }
    }

    public void FadeMusicIn(float time)
    {
        if (!DOTween.IsTweening(musicSource))
        {
            musicSource.DOFade(defaultMusicVolume, time);
        }
        else
        {
            musicSource.DOComplete();
        }
    }

    public void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    // public void ToggleMusic()
    // {
    //     int currentMusicMute = PlayerPrefs.GetInt("musicmute");

    //     if (currentMusicMute == 0)
    //     {
    //         // MUTE BUTTON
    //         musicSource.mute = true;
    //         // musicMute.sprite = musicMutedSprite;
    //         PlayerPrefs.SetInt("musicmute", 1);
    //     }
    //     else if (currentMusicMute == 1)
    //     {
    //         // UNMUTE BUTTON
    //         musicSource.mute = false;
    //         // musicMute.sprite = musicUnmutedSprite;
    //         PlayerPrefs.SetInt("musicmute", 0);
    //     }
    // }

    // private void SetMuteStatus()
    // {
    //     int currentMusicMute = PlayerPrefs.GetInt("musicmute");

    //     if (currentMusicMute == 0)
    //     {
    //         // IS NOT MUTED
    //         musicSource.mute = false;
    //         musicMute.sprite = musicUnmutedSprite;
    //     }
    //     else if (currentMusicMute == 1)
    //     {
    //         // IS MUTED
    //         musicSource.mute = true;
    //         musicMute.sprite = musicMutedSprite;
    //     }
    // }

}

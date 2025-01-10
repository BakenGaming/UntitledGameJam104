using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public static class SoundManager
{
    public static event Action<GameObject> OnMusicPlayed;
    public enum Sound
    { 
        uiClick, playerShoot, playerDie, enemyTakeDamage, collect
    }

    public enum Music
    { 
        defaultMusic
    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject currentMusicObject;
    private static GameObject currentSoundObject;
    private static AudioSource currentAudioSource;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        //soundTimerDictionary[Sound.playerMove] = 0;
    }
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (currentSoundObject == null) 
            { 
                currentSoundObject = new GameObject("Sound");
                currentAudioSource = currentSoundObject.AddComponent<AudioSource>();
            } 
            currentAudioSource.PlayOneShot(GetAudioClip(sound));
            currentAudioSource.volume = GetClipVolume(sound);
        }
    }

    public static void PlayMusic(Music currentMusic)
    {
        if (currentMusicObject == null)
        {
            currentMusicObject = new GameObject("Music");
            AudioSource musicSource = currentMusicObject.AddComponent<AudioSource>();
            musicSource.clip = GetMusicTrack(currentMusic);
            musicSource.loop = true;
            musicSource.Play();
            musicSource.volume = GetMusicVolume(currentMusic);
        }
        else
        {
            currentMusicObject.GetComponent<AudioSource>().Stop();
            currentMusicObject.GetComponent<AudioSource>().clip = GetMusicTrack(currentMusic);
            currentMusicObject.GetComponent<AudioSource>().Play();
            currentMusicObject.GetComponent<AudioSource>().volume = GetMusicVolume(currentMusic);

        }
        OnMusicPlayed?.Invoke(currentMusicObject);
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            /*case (Sound.playerMove):
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .25f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else return false;
                }
                else return true;*/

        }
            
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.audioClipArray)
        {
            if (soundAudioClip.sound == sound) return soundAudioClip.audioClip;

        }

        Debug.LogError("Sound " + sound + " does not exist!");
        return null;
    }

    private static AudioClip GetMusicTrack(Music musicTrack)
    {
        foreach (GameAssets.MusicAudioClip musicAudioClip in GameAssets.i.musicTrackArray)
        {
            if (musicAudioClip.music == musicTrack) return musicAudioClip.audioClip;

        }

        return null;
    }

    private static float GetClipVolume(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.audioClipArray)
        {
            if (soundAudioClip.sound == sound) return soundAudioClip.volume;

        }

        Debug.LogError("Sound " + sound + " does not exist!");
        return 1f;

    }

    private static float GetMusicVolume(Music music)
    {
        foreach (GameAssets.MusicAudioClip musicAudioClip in GameAssets.i.musicTrackArray)
        {
            if (musicAudioClip.music == music) return musicAudioClip.volume;

        }

        Debug.LogError("Sound " + music + " does not exist!");
        return 1f;

    }

    public static void IncreaseTempo(AudioSource music) { music.pitch = 1.5f; }
    public static void ResetTempo(AudioSource music) { music.pitch = 1f; }
}

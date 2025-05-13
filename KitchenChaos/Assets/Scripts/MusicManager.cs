using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public static MusicManager Instance {get; private set;}
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private float volume = 1f;
    private AudioSource audioSource;

    private void Awake(){
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
        audioSource.volume = volume;
    }

    public void ChangeVolume(){
        volume += .1f;
        if(volume >= 1.1f){
            volume = 0;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
    }

    public float GetVolume(){
        return volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    #region audio

    public AudioMixer audioMixer;

    public void setMasterVolume(float volume){
        audioMixer.SetFloat("masterVolume", volume); // first parameter is the name of the exposed variable of the audio Mixer (set in Unity)
    }

    public void setMusicVolume(float volume){
        audioMixer.SetFloat("musicVolume", volume); // first parameter is the name of the exposed variable of the audio Mixer (set in Unity)
    }

    public void setEffectVolume(float volume){
        audioMixer.SetFloat("effectVolume", volume); // first parameter is the name of the exposed variable of the audio Mixer (set in Unity)
    }

    #endregion

}

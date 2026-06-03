using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class sAudioManager : MonoBehaviour
{
    public static sAudioManager audioManagerGlobal;

    List<AudioSource> activeAudioSources;

    public AudioMixer audioMixer;
    public AudioMixerGroup mixerGroupMusic, mixerGroupSFX, mixerGroupUI, mixerGroupAmbience;

    private void Awake()
    {
        if (audioManagerGlobal == null)
            audioManagerGlobal = this;
        else
            Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeAudioSources = new List<AudioSource>();
    }

    public void SceneChange()
    {
        activeAudioSources.Clear();
        activeAudioSources = new List<AudioSource>();
    }

    public void AddAudioSource(AudioSource _audioSource)
    {
        CheckActiveAudio();
        activeAudioSources.Add(_audioSource);
    }

    public void RemoveAudioSource(AudioSource _audioSource)
    {
        activeAudioSources.Remove(_audioSource);
    }

    void CheckActiveAudio()
    {
        //Debug.Log("Checking Active Audio");

        //if(activeAudioSources.Count > 0)

        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            if(activeAudioSources[i] == null)
            {
                activeAudioSources.RemoveAt(i);
                return;
            }

            if (!activeAudioSources[i].isPlaying)
            {
                activeAudioSources.Remove(activeAudioSources[i]);
            }
        }
    }

    public void StopAllAudio()
    {
        // iterates through all audio sources
        foreach (AudioSource audioSource in activeAudioSources)
        {
            // stops the audio
            audioSource.Stop();
        }
    }

    // MIXER CONTROLS FOR UI TO CALL - Move this to it's own script

    public void ChangeMusicVolume(float _newValue)// Changes fader value of music volume
    {
        _newValue = Mathf.Clamp(_newValue, 0.1f, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(_newValue) * 20);// Changes as a logarithmic fade

    }

    public void ChangeSFXVolume(float _newValue)// Changes fader value of sfx volume
    {
        _newValue = Mathf.Clamp(_newValue, 0.1f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(_newValue) * 20);

    }

    public void ChangeMasterVolume(float _newValue)// Changes fader value of master volume
    {
        _newValue = Mathf.Clamp(_newValue, 0.1f, 1f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_newValue) * 20);

    }

    public void ChangeAmbienceVolume(float _newValue)// Changes fader value of master volume
    {
        _newValue = Mathf.Clamp(_newValue, 0.1f, 1f);
        audioMixer.SetFloat("AmbienceVolume", Mathf.Log10(_newValue) * 20);

    }
}

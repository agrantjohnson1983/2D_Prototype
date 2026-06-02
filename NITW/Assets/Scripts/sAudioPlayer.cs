using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum eAudioMixerType { music, sfx, ui, ambience }
public enum eSFXTriggerType { eSFXtriggerBasic, eSFXtriggerRandom, eSFXtriggerOrdered,  }

public class sAudioPlayer : MonoBehaviour
{
    public AudioSource sfxSource, musicSource, uiSource, ambienceSource;

    public SO_AudioData[] audioDataCollection;

    public static sAudioPlayer audioPlayerGlobal;

    private void Awake()
    {
        if(audioPlayerGlobal == null) 
            audioPlayerGlobal = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //if(audioDataCollection.Length > 0)
        foreach(SO_AudioData _audioData in audioDataCollection)
        {
            //Debug.Log("Setting up audio collection");

            // this will setup the Dictionarys so the audio cues work
            //_audioData.SetupAudio();
        }
    }

    public void TriggerSFX(string _cueName, eSFXTriggerType _triggerType, eAudioMixerType _mixType)
    {
        //Debug.Log("Audio Player Triggering SFX cue: " + _cueName);

        // temp trigger source set to null to start
        AudioSource triggerSource = null;

        // temp audio data set to null to start
        SO_AudioData audioData = null;

        // iterates through audio data collection
        foreach(SO_AudioData a in audioDataCollection)
        {
            // checks if tag is in dictionary
            if(a.ReturnSoundEffectFromTag(_cueName) != null)
            {
                // if the sfx returns then it sets temp audio data
                audioData = a;
            }

            else
            {
                Debug.Log("Did not find sound in " + a + " data with cue " + _cueName);
            }
        }

        // returns if audio data is still null
        if (audioData == null)
        {
            Debug.Log("audio data is still null - did not find " + _cueName + " in list");
            return;
        }
        
        // this sets the audioSource based on the mixer type
        switch (_mixType)
        {
            case eAudioMixerType.sfx:

                triggerSource = sfxSource;

                break;

            case eAudioMixerType.ui:

                triggerSource = uiSource;

                break;

            case eAudioMixerType.music:

                triggerSource = musicSource;

                break;

            case eAudioMixerType.ambience:

                triggerSource = ambienceSource;

                break;
        }

        // this determines what kind of audio trigger will be played
        switch(_triggerType)
        {
            case eSFXTriggerType.eSFXtriggerBasic:

                audioData.TriggerAudio(_cueName, triggerSource);

                break;

            case eSFXTriggerType.eSFXtriggerOrdered:

                audioData.TriggerAudioOrdered(_cueName, triggerSource);

                break;

            case eSFXTriggerType.eSFXtriggerRandom:

                audioData.TriggerAudioRandom(_cueName, triggerSource);

                break;
        }
    }
}

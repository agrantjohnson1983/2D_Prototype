using System.Collections.Generic;
using UnityEngine;

/*
 This script handles audio data and also handles the audio trigger and audiosource playings

 To Use:
 Import script into Unity and Create an SO_Audio scriptable object
 Create and add a Public SO_Audio reference to a Monobehavior script and drag in the scriptable object reference
 Select the scriptable object reference and observe the Sfx list in the inspector
 Click the "+" or fill out the number to populate the SoundEffect array
 Each SoundEffect takes in an array of AudioClips that must be populated clicking the "+" or adding in a number.
 The tag is used as an argument within the Audio Trigger methods to trigger audio cues.
 Volume can be set or randomized within the MinVolume and MaxVolume range sliders.
 Pitch can be randomized within the MinPitch and MaxPitch range sliders.
 Allow Overplay can be toggled on to allow instances of sfx to play over each other verses cutting each other off
 No Interrupt can be toggled to make new instances of sfx not play if a current instance is playing

 There are multiple audio trigger types : 
 Trigger Audio is used for regular single clip sfx
 Trigger Audio Random is used for arrays of multiple clips that need to randomize
 Trigger Audio Ordered is used for playing audio clips in the sequential order of the array - this also includes a reset method to set the index at 0 and start the order over

 The triggers feed through the Play method which handles volume, pitch and overplay/interrupt logic

 There is an sAudioManager script that owns audio on a higher level and controls the mixers
 There is an sMusicPlayer script that can be used for music controls, crossfading
*/

[CreateAssetMenu(fileName = "SO_Audio", menuName = "Scriptable Objects/SO_Audio")]
public class SO_AudioData : ScriptableObject
{
    // Class for each sound effect within the SO
    [System.Serializable]
    public class SoundEffect
    {
        // Dictionary tag name
        public string tag;

        // Audioclip array
        public AudioClip [] audioClips;

        [HideInInspector]
        public int currentIndex = 0; 

        // volume
        [Range(0f, 1f)] public float volume = 0.5f;

        // volume randomization
        public bool randomizeVolume = false;
        [Range(0f, 1f)] public float minVolume = 0.4f;
        [Range(0f, 1f)] public float maxVolume = 0.8f;

        //pitch randomization
        public bool randomizePitch = false;
        [Range(-3, 3)] public float minPitch = 0.8f;
        [Range(-3, 3)] public float maxPitch = 1.2f;

        // this allows sfx to overlap each other in playback - otherwise only the newest instance will play, stopping the old one
        public bool allowOverplay = true;

        // this will prevent a sfx from playing if the current audioSource is playing - be careful with this one!
        public bool noInterrupt = false;
    }

    // List of Sound effects within SO
    public List<SoundEffect> sfxList;

    // Dictionary of sfx used as injectible
    Dictionary<string, SoundEffect> sfxDictionary;

    private void OnEnable()
    {
        if (sfxDictionary == null)
        {
            sfxDictionary = new Dictionary<string, SoundEffect>();
        }

        InitAudio();
    }

    // Gets called at start to inject the sfx
    public void InitAudio()
    {
        // checks to see that sfx list is poplulated
        if (sfxList != null)

            // iterates through each sfx in list
            foreach (SoundEffect sfx in sfxList)
            {
                // adds each sfx to dictionary
                sfxDictionary.Add(sfx.tag, sfx);
            }
    }

    // returns SoundEffect from Dictionary tag
    public SoundEffect ReturnSoundEffectFromTag(string _tag)
    {
        if (sfxDictionary.ContainsKey(_tag))
            return sfxDictionary[_tag];
        else
            return null;
    }

    // Use this to return audio clips from the dictionary with a tag
    AudioClip[] GetAudioClips(SoundEffect _sfx)
    {
        return _sfx.audioClips;
    }


    // AUDIO TRIGGERS - There are a variety of triggers behaviors for different sfx needs


    // This triggers the audio event and passes on the audio source and returns an array of clips;
    public void TriggerAudio(string _tag, AudioSource _source)
    {
        // Gets sfx from tag
        SoundEffect _sfx = ReturnSoundEffectFromTag(_tag);

        // Returns if there are no sfx
        if (_sfx == null)
        {
            Debug.LogWarning("No sfx found with tag: " + _tag);
            return;
        }

        // gets audioClip
        AudioClip[] audioClips = GetAudioClips(_sfx);

        // checks if audioClips are null
        if (audioClips == null)
        {
            Debug.LogWarning("No audioclips found in " + _sfx);
            return;
        }

        // Plays audio - this should be for single clip only cues - this is why the 0 index is hard coded
        PlayAudio(_sfx, _source, audioClips[0]);     
    }

    // Use this to trigger audio clips in the order of the array
    public void TriggerAudioOrdered(string _tag, AudioSource _source)
    {
        // Gets the sfx from tag
        SoundEffect _sfx = ReturnSoundEffectFromTag(_tag);

        // Returns if there are no sfx
        if (_sfx == null)
        {
            Debug.LogWarning("No sfx found with tag: " + _tag);
            return;
        }

        // Gets audio array
        AudioClip[] audioClips = GetAudioClips(_sfx);

        // Returns if there are no clips
        if (audioClips == null)
            return;

        // Resets if max length is hit
        if(_sfx.currentIndex > audioClips.Length)
        {
            _sfx.currentIndex = 0;
        }

        // Plays current index clip
        PlayAudio(_sfx, _source, audioClips[_sfx.currentIndex]);

        // iterates on current index within the sfx AFTER it has played
        _sfx.currentIndex++;
    }

    // Use this to reset the index for an ordered trigger behavior if needed
    public void ResetTriggerOrdered(SoundEffect _sfx)
    {
        _sfx.currentIndex = 0;
    }

    // Use this to trigger a random audio clip from an array (different than pitch randomization)
    public void TriggerAudioRandom(string _tag, AudioSource _source)
    {
        // Gets the sfx from tag
        SoundEffect _sfx = ReturnSoundEffectFromTag(_tag);

        // Returns if there are no sfx
        if(_sfx == null)
        {
            Debug.LogWarning("No sfx found with tag: " + _tag);
            return;
        }

        // Gets audio clip array
        AudioClip[] audioClips = GetAudioClips(_sfx);

        // // Returns if there are no clips
        if (audioClips == null)
        {
            Debug.LogWarning("No audioclips found with sfx: " + _sfx);
            return;
        }

        // Plays random clip within the array
        PlayAudio(_sfx, _source, audioClips[Random.Range(0, audioClips.Length)]);
    }

    // This will set volume and handles randomization in volume level
    void SetVolume(SoundEffect _sfx, AudioSource _source)
    {
        // checks for volume randomization
        if (_sfx.randomizeVolume)
            _source.volume = Random.Range(_sfx.minVolume, _sfx.maxVolume);

        // if not randomized it sets it to the specified volume
        else
            _source.volume = _sfx.volume;
    }

    // This will set pitch and handles randomization
    void SetPitch(SoundEffect _sfx, AudioSource _source)
    {
        // checks for pitch randomization
        if (_sfx.randomizePitch)
            _source.pitch = Random.Range(_sfx.minPitch, _sfx.maxPitch);
    }

    // AUDIO PLAYER

    // This handles playing the audio clip - sets the volume, pitch then handles overplay & no interrupt
    void PlayAudio(SoundEffect _sfx, AudioSource _source, AudioClip _clip)
    {
        // checks for audio mgr
        if(sAudioManager.audioManagerGlobal != null)
        {
            // adds audioSource to audio mgr for master control
            sAudioManager.audioManagerGlobal.AddAudioSource(_source);
        }

        // Sets volume
        SetVolume(_sfx, _source);

        // Sets pitch
        SetPitch(_sfx, _source);

        // if no interrupt is on and audioSource is playing it returns before audio can be triggered
        if (_sfx.noInterrupt && _source.isPlaying)
        {
            return;
        }

        // plays one shot so audio can overlap - this won't work if no interrupt is on
        if (_sfx.allowOverplay)
        {
            _source.PlayOneShot(_clip);
        }

        // uses regular Play() which will cut off current clip
        else
        {
            _source.clip = _clip;
            _source.Play();
        }
    }
}

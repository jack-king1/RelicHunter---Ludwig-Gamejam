using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//https://gist.github.com/phosphoer/19a72675fed68c564bfd2bf3f7167575

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Dictionary<GameObject, AudioGroup> _audioGroups = new Dictionary<GameObject, AudioGroup>();
    public List<SoundBank> allAudio = new List<SoundBank>();
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public class AudioInstance
    {
        public AudioSource AudioSource;
        public SoundBank SoundBank;

        private int _lastRandomClip;


        public AudioClip GetNextRandomClip()
        {
            int index = Random.Range(0, SoundBank.AudioClips.Length);
            if (index == _lastRandomClip)
            {
                index = (index + 1) % SoundBank.AudioClips.Length;
            }

            _lastRandomClip = index;
            return SoundBank.AudioClips[index];
        }
    }

    private class AudioGroup
    {
        public int InstanceCount { get { return _instanceMap.Count; } }

        private Dictionary<SoundBank, AudioInstance> _instanceMap = new Dictionary<SoundBank, AudioInstance>();

        public void AddAudioInstance(AudioInstance audioInstance)
        {
            _instanceMap.Add(audioInstance.SoundBank, audioInstance);
        }

        public void RemoveAudioInstance(AudioInstance audioInstance)
        {
            _instanceMap.Remove(audioInstance.SoundBank);
        }

        public AudioInstance GetAudioInstance(SoundBank forSoundBank)
        {
            AudioInstance audioInstance = null;
            _instanceMap.TryGetValue(forSoundBank, out audioInstance);
            return audioInstance;
        }
    }

    // Used if you want to fade in a sound on start but audio manager doesn't exist yet
    public static IEnumerator QueueFadeInSoundRoutine(GameObject source, SoundBank soundBank, float toVolume, float duration)
    {
        while (Instance == null)
        {
            yield return null;
        }

        Instance.FadeInSound(source, soundBank, toVolume, duration);
    }

    // Used if you want to play a sound on start but audio manager doesn't exist yet  
    public static IEnumerator QueuePlaySoundRoutine(GameObject source, SoundBank soundBank, float volumeScale = 1.0f)
    {
        while (Instance == null)
        {
            yield return null;
        }

        Instance.PlaySound(source, soundBank, volumeScale);
    }

    // Fade in a sound over a time period, if the source gets destroyed it will be cancelled
    public Coroutine FadeInSound(GameObject source, SoundBank soundBank, float duration, float toVolume = 1.0f)
    {
        AudioInstance audioInstance = PlaySound(source, soundBank, 0);
        return StartCoroutine(FadeAudioRoutine(audioInstance, 0, toVolume * soundBank.VolumeScale, duration));
    }

    // Fade out a sound over a time period, if the source gets destroyed it will be cancelled
    public Coroutine FadeOutSound(GameObject source, SoundBank soundBank, float duration, bool playIfStopped = false)
    {
        AudioInstance audioInstance = GetOrAddAudioInstance(source, soundBank);
        if (!audioInstance.AudioSource.isPlaying && playIfStopped)
        {
            audioInstance.AudioSource.Play();
        }

        return StartCoroutine(FadeAudioRoutine(audioInstance, audioInstance.AudioSource.volume, 0, duration));
    }

    public void CancelFade(Coroutine fadeRoutine)
    {
        StopCoroutine(fadeRoutine);
    }

    // Play a sound on the global source (multiple simulataneous sounds supported)
    public void PlaySound(SoundBank soundBank, float volumeScale = 1.0f)
    {
        PlaySound(gameObject, soundBank, volumeScale);
    }

    // Play a sound from a specific source, use for spatial sounds or if you want to do things with the 
    // AudioInstance like modify the volume over time by accessing the unity AudioSource
    // There is always a unique audio source per simulatenous sound
    public AudioInstance PlaySound(GameObject source, SoundBank soundBank, float volumeScale = 1.0f)
    {
        AudioInstance audioInstance = GetOrAddAudioInstance(source, soundBank);
        if (audioInstance != null)
        {
            audioInstance.AudioSource.pitch = 1.0f + audioInstance.SoundBank.PitchOffset + audioInstance.SoundBank.RandomPitchOffSet();
            audioInstance.AudioSource.volume = audioInstance.SoundBank.VolumeScale * volumeScale;
            audioInstance.AudioSource.clip = audioInstance.GetNextRandomClip();
            audioInstance.AudioSource.Play();
        }
        else
        {
            Debug.LogWarning(string.Format("Couldn't find audio instance for {0}:{1}", source.name, soundBank.name));
        }

        return audioInstance;
    }

    // Stop a currently playing sound on the global source
    public void StopSound(SoundBank soundBank)
    {
        StopSound(this.gameObject, soundBank);
    }

    // Stop a currently playing sound on a specific source
    public void StopSound(GameObject source, SoundBank soundBank)
    {
        AudioInstance audioInstance = GetAudioInstance(source, soundBank);
        if (audioInstance != null)
        {
            audioInstance.AudioSource.Stop();
        }
        else
        {
            Debug.LogWarning(string.Format("Couldn't find audio instance for {0}:{1}", source.name, soundBank.name));
        }
    }

    private IEnumerator FadeAudioRoutine(AudioInstance audioInstance, float fromVolume, float toVolume, float duration)
    {
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            yield return null;

            float t = time / duration;
            if (audioInstance.AudioSource != null)
            {
                audioInstance.AudioSource.volume = Mathf.Lerp(fromVolume, toVolume, t);
            }
            else
            {
                yield break;
            }
        }

        audioInstance.AudioSource.volume = toVolume;
        if (toVolume == 0)
        {
            StopSound(audioInstance.AudioSource.gameObject, audioInstance.SoundBank);
        }
    }

    private AudioGroup GetAudioGroup(GameObject forSource)
    {
        AudioGroup audioGroup = null;
        _audioGroups.TryGetValue(forSource, out audioGroup);
        return audioGroup;
    }

    private AudioGroup GetOrAddAudioGroup(GameObject forSource)
    {
        AudioGroup audioGroup = GetAudioGroup(forSource);
        if (audioGroup == null)
        {
            audioGroup = new AudioGroup();
            _audioGroups.Add(forSource, audioGroup);
        }

        return audioGroup;
    }

    private AudioInstance GetAudioInstance(GameObject forSource, SoundBank forSoundBank)
    {
        AudioGroup audioGroup = GetAudioGroup(forSource);
        AudioInstance audioInstance = audioGroup.GetAudioInstance(forSoundBank);
        if (audioInstance.AudioSource == null)
        {
            audioGroup.RemoveAudioInstance(audioInstance);
            if (audioGroup.InstanceCount == 0)
            {
                _audioGroups.Remove(forSource);
            }
            return null;
        }

        return audioInstance;
    }

    // I didn't make much about the audio souces configurable, this is where you'd change the defaults
    private AudioInstance GetOrAddAudioInstance(GameObject forSource, SoundBank soundBank)
    {
        AudioGroup audioGroup = GetOrAddAudioGroup(forSource);
        AudioInstance audioInstance = audioGroup.GetAudioInstance(soundBank);
        if (audioInstance == null)
        {
            audioInstance = new AudioInstance();
            audioInstance.AudioSource = forSource.AddComponent<AudioSource>();
            audioInstance.AudioSource.playOnAwake = false;
            audioInstance.AudioSource.spatialize = soundBank.IsSpatial;
            audioInstance.AudioSource.spatialBlend = soundBank.IsSpatial ? 1.0f : 0.0f;
            audioInstance.AudioSource.volume = soundBank.VolumeScale;
            audioInstance.AudioSource.loop = soundBank.IsLooping;
            audioInstance.AudioSource.minDistance = soundBank.MinDistance;
            audioInstance.AudioSource.maxDistance = soundBank.MaxDistance;
            audioInstance.AudioSource.rolloffMode = AudioRolloffMode.Linear;
            audioInstance.AudioSource.outputAudioMixerGroup = soundBank.AudioMixerGroup;
            audioInstance.AudioSource.dopplerLevel = soundBank.VolumeScale;
            audioInstance.SoundBank = soundBank;
            audioGroup.AddAudioInstance(audioInstance);
        }

        return audioInstance;
    }

    public SoundBank GetSoundBank(string name)
    {
        foreach(SoundBank sb in allAudio)
        {
            if(sb.soundName == name)
            {
                return sb;
            }
        }

        return null;
    }
}
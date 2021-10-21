using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "new-sound-bank", menuName = "Sound Bank")]
public class SoundBank : ScriptableObject
{
    public string soundName;
    public AudioClip[] AudioClips;
    public bool IsSpatial;
    public float MaxDistance = 100.0f;
    public float MinDistance = 10.0f;
    public float VolumeScale = 1.0f;
    public float DopplerLevel = 0;
    public float PitchOffset = 0;
    public float pitchOffsetMin = 0;
    public float pitchOffsetMax = 0;
    public bool IsLooping;
    public AudioMixerGroup AudioMixerGroup;

    public AudioClip RandomClip
    {
        get
        {
            return AudioClips[Random.Range(0, AudioClips.Length)];
        }
    }

    public float RandomPitchOffSet()
    {
        return Random.Range(pitchOffsetMin, pitchOffsetMax);
    }
}


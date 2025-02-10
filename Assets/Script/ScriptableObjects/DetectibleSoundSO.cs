using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DetectibleSoundSO : ScriptableObject
{
    public string soundName;
    public List<AudioClip> clips;
    public AudioClip clip
    {
        get { return clips[Random.Range(0, clips.Count)]; }
    }
    public float detectionRadius;
}

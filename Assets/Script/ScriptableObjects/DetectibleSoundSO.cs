using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DetectibleSoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public float detectionRadius;
}

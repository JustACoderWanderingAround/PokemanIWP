using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DetectibleSound : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public float detectionRadius;
}

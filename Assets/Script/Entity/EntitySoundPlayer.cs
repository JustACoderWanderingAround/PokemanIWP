using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EntitySoundPlayer : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> m_Sounds = new List<AudioClip>();
    
    public void Play(int soundID)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = m_Sounds[soundID];
        audioSource.Play();
    }
}

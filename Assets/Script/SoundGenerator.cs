using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundGenerator : MonoBehaviour
{
    public List<DetectibleSoundSO> sounds = new List<DetectibleSoundSO>();
    AudioSource audioSource;
    bool loopSoundGenerator;
    IEnumerator EmitSoundLoop(int soundID, float frequency)
    {
        WaitForSeconds wait = new WaitForSeconds(frequency);
        while (loopSoundGenerator)
        {
            yield return wait;

            EmitSound(soundID);
        }
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySoundOnce(int soundID, bool interruptSource = false, bool listenable = true)
    {
        audioSource.clip = sounds[soundID].clip;
        audioSource.loop = false;
        loopSoundGenerator = false;
        if (!audioSource.isPlaying || interruptSource)
            audioSource.Play();
        if (listenable)
            EmitSound(soundID); 
    }
    public void PlaySoundOnce(AudioClip newSound, bool interruptSource = false)
    {
        audioSource.clip = newSound;
        audioSource.loop = false;
        loopSoundGenerator = false;
        if (!audioSource.isPlaying || interruptSource)
            audioSource.Play();
    }
    public void PlaySoundLoop(int soundID, float frequency)
    {
        audioSource.clip = sounds[soundID].clip;
        loopSoundGenerator = true;
        StartCoroutine(EmitSoundLoop(soundID, frequency));
    }
    public void StopSoundLoop(int soundID, float frequency)
    {
        if (audioSource.clip == sounds[soundID].clip)
        {
            audioSource.Stop();
            audioSource.clip = null;
            loopSoundGenerator = false;
            StopCoroutine(EmitSoundLoop(soundID, frequency));
        }
    }
    public void StopAllSounds()
    {
        audioSource.Stop();
        audioSource.clip = null;
        loopSoundGenerator = false;
        StopAllCoroutines();
    }
    void EmitSound(int soundID) 
    {
        //Debug.Log("Sound emitted at " + transform.position);
        if (audioSource.clip != sounds[soundID].clip)
            audioSource.clip = sounds[soundID].clip;
        if (!audioSource.isPlaying)
            audioSource.Play();
        Collider[] potentialListeners = Physics.OverlapSphere(transform.position, sounds[soundID].detectionRadius);
        foreach (var c in potentialListeners)
        {
            if (c.gameObject.TryGetComponent(out ISoundListener listener))
            {
                listener.OnSoundHeard(transform.position);
            }
        }
    }
}

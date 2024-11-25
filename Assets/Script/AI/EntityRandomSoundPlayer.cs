using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class EntityRandomSoundPlayer : MonoBehaviour
{

    [SerializeField]
    List<AudioClip> availableClips = new List<AudioClip>();
    [SerializeField]
    float maxSoundTimer = 15.0f;
    float soundTimer;
    AudioSource audioSource;
    [SerializeField]
    bool playOnAwake = false;

    private void Awake()
    {

        if (!playOnAwake)
            soundTimer = Random.Range(maxSoundTimer / 2, maxSoundTimer); 
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer < 0)
        {
            soundTimer = maxSoundTimer;
            audioSource.clip = availableClips[Random.Range(0, availableClips.Count)];
            audioSource.Play();
        }
    }
}

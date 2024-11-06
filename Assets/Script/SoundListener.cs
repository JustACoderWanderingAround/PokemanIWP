
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundListener : MonoBehaviour
{
    UnityEvent onSoundHeardCallback;

    public void OnSoundHeard()
    {
        onSoundHeardCallback.Invoke();
    }
}

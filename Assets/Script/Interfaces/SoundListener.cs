
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISoundListener
{
    public void OnSoundHeard(Vector3 soundPos);
}

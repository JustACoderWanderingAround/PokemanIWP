using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundMaker : MonoBehaviour
{
    SoundGenerator main_SG;
    // Start is called before the first frame update
    void Start()
    {
        main_SG = GetComponent<SoundGenerator>();
        main_SG.PlaySoundLoop(0, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

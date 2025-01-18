using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventPostOffice
{

    private static EventPostOffice instance;
    public static EventPostOffice Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventPostOffice();
            }
            return instance;
        }
    }

    
}

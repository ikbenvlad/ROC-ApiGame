using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class answer 
{
    public int status;
    public String message;
    public String payload;

    public void debugLog()
    {
        Debug.Log(status);
        Debug.Log(message);
        Debug.Log(payload);
    }
}
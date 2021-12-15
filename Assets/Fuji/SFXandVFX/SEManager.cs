using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SEManager : SingletonMonoBehaviour<SEManager>
{
    public SEData seData;
    /*
    private void Awake()
    {
        base.Awake();
        Debug.Log(seData.debugString);
    }
    */
    private void Start()
    {
    }
    public void PlaySE(string playSEName)
    {
        if(Array.IndexOf(seData.SEName,playSEName) != -1)
        {
            AudioSource.PlayClipAtPoint(seData.SEClip[Array.IndexOf(seData.SEName, playSEName)],Camera.main.transform.position);
        }
        else
        {
            Debug.LogError("SE with name " + playSEName + " doesn't exsist. Check SEData in Fuji/SFXandVFX folder.");
        }
    }
    public void PlaySEDebug()
    {
        AudioSource.PlayClipAtPoint(seData.SEClip[UnityEngine.Random.Range(0,seData.SEClip.Length)], Camera.main.transform.position);
    }
}

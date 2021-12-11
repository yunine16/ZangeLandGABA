using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SEData",menuName ="Fuji_Scriptable/CreateSEData")]
public class SEData : ScriptableObject
{
    public string[] SEName;
    public AudioClip[] SEClip;
}

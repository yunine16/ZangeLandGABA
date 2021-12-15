using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoundPrefs : MonoBehaviour
{
    public AudioSource bgmSource;
    public Slider bgmSlider, seSlider;
    public static string bgmVolumeKey = "b", seVolumeKey = "s";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Initialize");
    }
    IEnumerator Initialize()
    {
        if (PlayerPrefs.HasKey(bgmVolumeKey) == false)
        {
            PlayerPrefs.SetFloat(bgmVolumeKey, 0.2f);
        }
        if (PlayerPrefs.HasKey(seVolumeKey) == false)
        {
            PlayerPrefs.SetFloat(seVolumeKey, 0.2f);
        }
        yield return null;
        bgmSlider.value = PlayerPrefs.GetFloat(bgmVolumeKey);
        seSlider.value = PlayerPrefs.GetFloat(seVolumeKey);
        yield return null;
        bgmSource.PlayDelayed(0.2f);
    }
}

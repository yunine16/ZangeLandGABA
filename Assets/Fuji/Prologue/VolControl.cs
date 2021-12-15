using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class VolControl : MonoBehaviour
{
    private Volume volume;
    private VolumeProfile vProfile;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private ColorAdjustments colorAdjustments;
    public int stageProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        vProfile = volume.sharedProfile;
        vProfile.TryGet(out vignette);
        vProfile.TryGet(out filmGrain);
        vProfile.TryGet(out colorAdjustments);
        vignette.intensity.value = 0;
        filmGrain.intensity.value = 0.1f;
    }
    public void EnterZangeScreen()
    {
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, 0.4f, 3);
        DOTween.To(() => filmGrain.intensity.value, (x) => filmGrain.intensity.value = x, 0.7f, 3);
    }
}

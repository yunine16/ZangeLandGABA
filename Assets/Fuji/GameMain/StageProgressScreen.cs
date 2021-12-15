using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class StageProgressScreen : MonoBehaviour
{
    private Volume volume;
    private VolumeProfile vProfile;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private ColorAdjustments colorAdjustments;
    public int stageProgress = 0;
    private float initialVintensity, initialFintensity;
    [SerializeField] UIFunctionsForGame gameUIscript;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        vProfile = volume.sharedProfile;
        vProfile.TryGet(out vignette);
        vProfile.TryGet(out filmGrain);
        vProfile.TryGet(out colorAdjustments);
        initialVintensity = vignette.intensity.value;
        initialFintensity = filmGrain.intensity.value;
    }
    private void Update()
    {
        vignette.intensity.value = Mathf.Clamp(initialVintensity - (initialVintensity * stageProgress / 100f),0,initialVintensity);
        filmGrain.intensity.value = Mathf.Clamp(initialFintensity - (initialFintensity * stageProgress / 100f), 0, initialFintensity);
        if(stageProgress >= 100)
        {
            gameUIscript.ShrinkBlackArea();
        }
    }
}

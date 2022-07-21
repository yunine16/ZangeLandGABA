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
    public int stageProgress = 0;
    private float initialVintensity, initialFintensity;
    [SerializeField] UIFunctionsForGame gameUIscript;
    // Start is called before the first frame update
    void Start()
    {
        //パラメータを取得、初期値を設定
        volume = GetComponent<Volume>();
        vProfile = volume.sharedProfile;
        vProfile.TryGet(out vignette);
        vProfile.TryGet(out filmGrain);
        vignette.intensity.value = 0.4f;
        filmGrain.intensity.value = 0.7f;
        initialVintensity = 0.4f;
        initialFintensity = 0.7f;
    }
    private void Update()
    {
        //ステージ進行に伴い画面へのエフェクトがなくなっていく
        //ステージ進行100でちょうどなくなるように
        float vValue = Mathf.Clamp(initialVintensity - (initialVintensity * stageProgress / 100f), 0, initialVintensity);
        float fValue = Mathf.Clamp(initialFintensity - (initialFintensity * stageProgress / 100f), 0, initialFintensity);
        //Tweenで滑らかに
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, vValue, 5);
        DOTween.To(() => filmGrain.intensity.value, (x) => filmGrain.intensity.value = x, fValue, 5);

        
        //ステージ進捗が100以上ならゲーム終了、画面横の黒い部分が縮む
        if(stageProgress >= 100)
        {
            gameUIscript.ShrinkBlackArea();
        }
    }
}

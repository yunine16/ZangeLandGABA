using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class VolControl : MonoBehaviour
{
    private Volume volume;
    private VolumeProfile vProfile;
    private Vignette vignette;
    private FilmGrain filmGrain;
    // Start is called before the first frame update
    void Start()
    {
        //各種Volumeの項目を取得、初期値を設定
        volume = GetComponent<Volume>();
        vProfile = volume.sharedProfile;
        vProfile.TryGet(out vignette);
        vProfile.TryGet(out filmGrain);
        //タイトル画面とプロローグ画面とで画面エフェクトの初期値が異なる。
        if (SceneManager.GetActiveScene().name == "Fuji_Title")
        {
            vignette.intensity.value = 0.4f;
            filmGrain.intensity.value = 0.7f;
        }
        else if (SceneManager.GetActiveScene().name == "Fuji_Prologue")
        {
            vignette.intensity.value = 0;
            filmGrain.intensity.value = 0.1f;
        }
    }
    public void EnterZangeScreen()
    {
        //ザンゲランド突入時、画面ザラザラと四隅の黒をTween
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, 0.4f, 3);
        DOTween.To(() => filmGrain.intensity.value, (x) => filmGrain.intensity.value = x, 0.7f, 3);
    }
}

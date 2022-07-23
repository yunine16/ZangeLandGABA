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
        //�e��Volume�̍��ڂ��擾�A�����l��ݒ�
        volume = GetComponent<Volume>();
        vProfile = volume.sharedProfile;
        vProfile.TryGet(out vignette);
        vProfile.TryGet(out filmGrain);
        //�^�C�g����ʂƃv�����[�O��ʂƂŉ�ʃG�t�F�N�g�̏����l���قȂ�B
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
        //�U���Q�����h�˓����A��ʃU���U���Ǝl���̍���Tween
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, 0.4f, 3);
        DOTween.To(() => filmGrain.intensity.value, (x) => filmGrain.intensity.value = x, 0.7f, 3);
    }
}

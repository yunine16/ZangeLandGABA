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
        //�p�����[�^���擾�A�����l��ݒ�
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
        //�X�e�[�W�i�s�ɔ�����ʂւ̃G�t�F�N�g���Ȃ��Ȃ��Ă���
        //�X�e�[�W�i�s100�ł��傤�ǂȂ��Ȃ�悤��
        float vValue = Mathf.Clamp(initialVintensity - (initialVintensity * stageProgress / 100f), 0, initialVintensity);
        float fValue = Mathf.Clamp(initialFintensity - (initialFintensity * stageProgress / 100f), 0, initialFintensity);
        //Tween�Ŋ��炩��
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, vValue, 5);
        DOTween.To(() => filmGrain.intensity.value, (x) => filmGrain.intensity.value = x, fValue, 5);

        
        //�X�e�[�W�i����100�ȏ�Ȃ�Q�[���I���A��ʉ��̍����������k��
        if(stageProgress >= 100)
        {
            gameUIscript.ShrinkBlackArea();
        }
    }
}

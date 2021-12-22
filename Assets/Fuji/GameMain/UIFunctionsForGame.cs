using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIFunctionsForGame : MonoBehaviour
{
    [SerializeField] private Image pauseImage,failureImage,fadeBlack,fadeWhite,blackAreaRight,blackAreaLeft,crackImage;
    [SerializeField] private ShootingManager shootingManager;
    private bool pausing = false,
                 changingSEVolume = false,
                 initialized = false,
                 loadStarted;
    private AsyncOperation clearScene,titleScene;
    private SoundPrefs soundPrefs;
    private AudioSource myAudioSource;
    [SerializeField] private Material crackMat;
    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        soundPrefs = GetComponent<SoundPrefs>();
        StartCoroutine("FadeIn");
        StartCoroutine("PreloadClearScene");
    }
    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.8f);
        fadeBlack.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        myAudioSource.Play();
    }
    IEnumerator PreloadClearScene()
    {
        if (!loadStarted)
        {
            clearScene = SceneManager.LoadSceneAsync("Fuji_Clear");
            clearScene.allowSceneActivation = false;
            titleScene = SceneManager.LoadSceneAsync("Fuji_Title");
            titleScene.allowSceneActivation = false;
            loadStarted = true;
        }
        return null;
    }
    private void Update()
    {
        //StartでseSliderの値が変更されているため、一回だけクリックすると音が出てしまう。
        //Updateの1フレーム目にchangingSEvolumeをfalseにする。
        if (!initialized)
        {
            changingSEVolume = false;
            initialized = true;
        }
        if (Input.GetMouseButtonUp(0) && changingSEVolume)
        {
            SEManager.Instance.PlaySE("SEVolumeChanged");
            changingSEVolume = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }
    }
    public void PointerEnter()
    {
        SEManager.Instance.PlaySE("MouseOver");
    }
    public void SetBGMVolume()
    {
        PlayerPrefs.SetFloat(SoundPrefs.bgmVolumeKey, soundPrefs.bgmSlider.value);
        myAudioSource.DOFade(PlayerPrefs.GetFloat(SoundPrefs.bgmVolumeKey), 0.2f);
    }
    public void SetSEVolume()
    {
        PlayerPrefs.SetFloat(SoundPrefs.seVolumeKey, soundPrefs.seSlider.value);
        changingSEVolume = true;
    }
    public void PauseButton()
    {
        SEManager.Instance.PlaySE("PauseButton");
        if (pausing)
        {
            pauseImage.rectTransform.DOScaleY(0, 0.2f).SetEase(Ease.OutExpo);
            pausing = false;
        }
        else
        {
            pauseImage.rectTransform.DOScaleY(1, 0.2f).SetEase(Ease.OutExpo);
            pausing = true;
        }
    }
    public void TitleButton()
    {
        fadeBlack.DOFade(1, 1);
        StartCoroutine("AllowChangeSceneToTitle");
    }
    IEnumerator AllowChangeSceneToTitle()
    {
        yield return new WaitForSeconds(1);
        titleScene.allowSceneActivation = true;
    }
    public void Failure()
    {
        failureImage.rectTransform.DOScaleY(1, 0.2f).SetEase(Ease.OutExpo);
    }
    public void Revenge()
    {
        failureImage.rectTransform.DOScaleY(0, 0.2f).SetEase(Ease.OutExpo);
        shootingManager.Revenge();
        crackImage.DOFade(0, 0.5f);
    }
    public void ShrinkBlackArea()
    {
        blackAreaLeft.rectTransform.DOScaleX(0, 3).SetEase(Ease.Linear);
        blackAreaRight.rectTransform.DOScaleX(0, 3).SetEase(Ease.Linear);
    }
    public void Success()
    {
        StartCoroutine("FadeAndChangeScene");
    }
    IEnumerator FadeAndChangeScene()
    {
        yield return new WaitForSeconds(3);
        fadeWhite.DOFade(1, 2);
        SEManager.Instance.PlaySE("EndLight");
        myAudioSource.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        clearScene.allowSceneActivation = true;
    }
}

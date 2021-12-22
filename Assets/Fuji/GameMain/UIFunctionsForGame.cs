using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIFunctionsForGame : MonoBehaviour
{
    [SerializeField] private Image pauseImage,failureImage,fadeImage,blackAreaRight,blackAreaLeft,crackImage;
    [SerializeField] private Button pauseButton;
    [SerializeField] private ShootingManager shootingManager;
    private bool pausing = false,
                 changingSEVolume = false,
                 initialized = false,
                 loadStarted;
    private AsyncOperation clearScene;
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
        fadeImage.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        myAudioSource.Play();
    }
    IEnumerator PreloadClearScene()
    {
        if (!loadStarted)
        {
            clearScene = SceneManager.LoadSceneAsync("Fuji_Clear");
            clearScene.allowSceneActivation = false;
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
            pauseButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo);
            pauseImage.rectTransform.DOScaleY(0, 0.2f).SetEase(Ease.OutExpo);
            pausing = false;
        }
        else
        {
            pauseButton.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutExpo);
            pauseImage.rectTransform.DOScaleY(1, 0.2f).SetEase(Ease.OutExpo);
            pausing = true;
        }
    }
    public void Failure()
    {
        failureImage.rectTransform.DOScaleY(1, 0.2f).SetEase(Ease.OutExpo);
        crackMat.SetInt(crackMat.shader.GetPropertyNameId(1), Random.Range(20, 40));
        crackImage.DOFade(1, 0.1f);
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
        fadeImage.DOFade(1, 1);
        myAudioSource.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        clearScene.allowSceneActivation = true;
    }
}

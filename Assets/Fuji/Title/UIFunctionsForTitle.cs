using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class UIFunctionsForTitle : MonoBehaviour
{
    private bool loadStarted = false,
                 changingSEVolume = false,
                 showingCredit = false;
    private SoundPrefs soundPrefs;
    private AsyncOperation prologueScene;
    [SerializeField] private Image fadeImage, creditImage;
    [SerializeField] private TextMeshProUGUI creditText;
    private void Start()
    {
        fadeImage.DOFade(0, 1);
        soundPrefs = GetComponent<SoundPrefs>();
        StartCoroutine("PreloadPrologueScene");
    }
    IEnumerator PreloadPrologueScene()
    {
        if (!loadStarted)
        {
            prologueScene = SceneManager.LoadSceneAsync("Fuji_Prologue");
            prologueScene.allowSceneActivation = false;
            loadStarted = true;
        }
        return null;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            changingSEVolume = false;
        }
        if (Input.GetMouseButtonUp(0) && changingSEVolume)
        {
            SEManager.Instance.PlaySE("SEVolumeChanged");
            changingSEVolume = false;
        }
    }
    public void PointerEnter()
    {
        SEManager.Instance.PlaySE("MouseOver");
    }
    public void SetBGMVolume()
    {
        PlayerPrefs.SetFloat(SoundPrefs.bgmVolumeKey, soundPrefs.bgmSlider.value);
        soundPrefs.bgmSource.DOFade(PlayerPrefs.GetFloat(SoundPrefs.bgmVolumeKey), 0.2f);
    }
    public void SetSEVolume()
    {
        PlayerPrefs.SetFloat(SoundPrefs.seVolumeKey, soundPrefs.seSlider.value);
        changingSEVolume = true;
    }
    public void StartButton()
    {
        StartCoroutine("FadeAndChangeScene");
        SEManager.Instance.PlaySE("StartButton");
    }
    IEnumerator FadeAndChangeScene()
    {
        fadeImage.DOFade(1, 1);
        soundPrefs.bgmSource.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        prologueScene.allowSceneActivation = true;
    }
    public void CreditButton()
    {
        SEManager.Instance.PlaySE("CreditButton");
        if (showingCredit)
        {
            creditImage.rectTransform.DOScaleY(0, 0.5f).SetEase(Ease.OutExpo);
            showingCredit = false;
            creditText.text = "クレジット";
        }
        else
        {
            creditImage.rectTransform.DOScaleY(1, 0.5f).SetEase(Ease.OutExpo);
            showingCredit = true;
            creditText.text = "閉じる";
        }
    }
}

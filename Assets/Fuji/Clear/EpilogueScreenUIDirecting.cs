using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class EpilogueScreenUIDirecting : MonoBehaviour
{
    //インスペクタで設定できるようになる
    [System.Serializable]
    //名前と音データをもつクラス
    public class narrationInfo
    {
        public string narrationText;
        public AudioClip audioClip;
        public bool italic;
    }
    [SerializeField] private narrationInfo[] narrationInfos;
    [SerializeField] private Image fadeWhite, fadeBlack;
    private AudioSource myAudioSource;
    private AsyncOperation titleScene;
    [SerializeField] private TextMeshProUGUI textArea, endText;
    private bool playing = false, loadStarted = false, gameEnded = false;
    [SerializeField] private float speed = 0.1f;
    private int nextText = 0;
    private void Awake()
    {
        for (int i = 0; i < narrationInfos.Length; i++)
        {
            narrationInfos[i].narrationText = narrationInfos[i].narrationText.Replace("\\n", "\n");
        }
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.DOFade(PlayerPrefs.GetFloat(SoundPrefs.bgmVolumeKey), 0.1f);
    }
    void Start()
    {
        StartCoroutine("ShowTextOneByOne", narrationInfos[0].narrationText);
        nextText++;
        StartCoroutine("PreloadTitleScene");
        StartCoroutine("FadeIn");
    }
    IEnumerator PreloadTitleScene()
    {
        if (!loadStarted)
        {
            titleScene = SceneManager.LoadSceneAsync("Fuji_Title");
            titleScene.allowSceneActivation = false;
            loadStarted = true;
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playing == false)
        {
            if (nextText < narrationInfos.Length)
            {
                if (narrationInfos[nextText].italic)
                {
                    textArea.fontStyle = FontStyles.Italic | FontStyles.Bold;
                }
                else
                {
                    textArea.fontStyle = FontStyles.Normal;
                }
                if (narrationInfos[nextText].audioClip != null)
                {
                    AudioSource.PlayClipAtPoint(narrationInfos[nextText].audioClip, Camera.main.transform.position,PlayerPrefs.GetFloat(SoundPrefs.seVolumeKey));
                }
                StartCoroutine("ShowTextOneByOne", narrationInfos[nextText].narrationText);
                nextText++;
                SEManager.Instance.PlaySE("Click");

                if(nextText == 2)
                {
                    fadeWhite.DOFade(0, 1);
                    myAudioSource.DOFade(PlayerPrefs.GetFloat(SoundPrefs.bgmVolumeKey), 0.1f);
                    myAudioSource.Play();
                }
            }
            else
            {
                StartCoroutine("EndText");
            }
            if (gameEnded)
            {
                StartCoroutine("BackToTitle");
            }

        }
    }
    IEnumerator EndText()
    {
        textArea.DOFade(0, 3);
        yield return new WaitForSeconds(3);
        endText.DOFade(1, 3);
        gameEnded = true;
    }
    IEnumerator BackToTitle()
    {
        fadeBlack.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        titleScene.allowSceneActivation = true;
    }
    IEnumerator ShowTextOneByOne(string narrationText)
    {
        playing = true;
        float time = 0;
        int lentemp = 0;
        while (true)
        {
            time += Time.deltaTime;
            yield return 0;
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            int len = Mathf.FloorToInt(time / speed);
            if (len > narrationText.Length)
            {
                break;
            }
            textArea.text = narrationText.Substring(0, len);
            if (len > lentemp)
            {
                SEManager.Instance.PlaySE("ShowingText");
                lentemp = len;
            }
        }
        textArea.text = narrationText;
        yield return 0;
        playing = false;
    }
}

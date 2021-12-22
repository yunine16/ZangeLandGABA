using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PrologueScreenUIDirecting : MonoBehaviour
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
    [SerializeField] private Image fadeScreen;
    private AsyncOperation gameScene;
    [SerializeField] private TextMeshProUGUI textArea;
    [SerializeField] private Image blackAreaRight, blackAreaLeft;
    private bool playing = false,loadStarted = false, changeSceneExecuted = false;
    [SerializeField] private float speed = 0.1f;
    private int nextText = 0;
    [SerializeField] private int enterZangeScreen = 0;
    [SerializeField] private AudioClip[] BGMs;
    [SerializeField] private VolControl volControl;
    private AudioSource myAudioSource;
    private void Awake()
    {
        for(int i = 0; i < narrationInfos.Length; i++)
        {
            narrationInfos[i].narrationText = narrationInfos[i].narrationText.Replace("\\n", "\n");
        }
    }
    void Start()
    {
        fadeScreen.DOFade(0, 1);
        myAudioSource = GetComponent<AudioSource>();
        StartCoroutine("ShowTextOneByOne", narrationInfos[0].narrationText);
        nextText++;
        StartCoroutine("PreloadPrologueScene");
        myAudioSource.clip = BGMs[0];
        myAudioSource.Play();
        myAudioSource.DOFade(PlayerPrefs.GetFloat(SoundPrefs.bgmVolumeKey), 1);
    }
    IEnumerator PreloadPrologueScene()
    {
        if (!loadStarted)
        {
            gameScene = SceneManager.LoadSceneAsync("Fuji_Game");
            gameScene.allowSceneActivation = false;
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
            }
            else
            {
                if (changeSceneExecuted == false)
                {
                    StartCoroutine("FadeAndChangeScene");
                    changeSceneExecuted = true;
                }
            }
            if(nextText == enterZangeScreen +1)
            {
                blackAreaLeft.rectTransform.DOScaleX(1, 2).SetEase(Ease.Linear);
                blackAreaRight.rectTransform.DOScaleX(1, 2).SetEase(Ease.Linear);
                volControl.EnterZangeScreen();
                myAudioSource.DOFade(0, 1);
            }
            if (nextText == enterZangeScreen + 2)
            {
                myAudioSource.clip = BGMs[1];
                myAudioSource.DOFade(PlayerPrefs.GetFloat(SoundPrefs.bgmVolumeKey), 1);
                myAudioSource.Play();
            }
        }
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
    IEnumerator FadeAndChangeScene()
    {
        fadeScreen.DOFade(1, 3);
        myAudioSource.DOFade(0, 2);
        yield return new WaitForSeconds(3);
        gameScene.allowSceneActivation = true;
    }
}

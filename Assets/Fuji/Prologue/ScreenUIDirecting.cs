using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScreenUIDirecting : MonoBehaviour
{
    //インスペクタで設定できるようになる
    [System.Serializable]
    //名前と音データをもつクラス
    public class SEInfo
    {
        public int SEtiming;
        public AudioClip audioClip;
    }
    [SerializeField] private SEInfo[] seInfos;

    //名前指定で再生するために、名前と音を結び付けるDictionaryを作成
    private Dictionary<int, AudioClip> SEDictionary = new Dictionary<int, AudioClip>();

    private AsyncOperation gameScene;
    [SerializeField] private string[] narrationTexts;
    [SerializeField] private TextMeshProUGUI textArea;
    [SerializeField] private Image blackAreaRight, blackAreaLeft;
    private bool playing = false,loadStarted = false;
    [SerializeField] private float speed = 0.1f;
    private int nextText = 0;
    [SerializeField] private int enterZangeScreen = 0;
    [SerializeField] private VolControl volControl;
    private void Awake()
    {
        //辞書に関連付け
        foreach (SEInfo seInfo in seInfos)
        {
            SEDictionary.Add(seInfo.SEtiming, seInfo.audioClip);
        }
    }
    void Start()
    {
        StartCoroutine("ShowTextOneByOne", narrationTexts[0]);
        nextText++;
        StartCoroutine("PreloadPrologueScene");
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
            if (nextText < narrationTexts.Length)
            {
                StartCoroutine("ShowTextOneByOne", narrationTexts[nextText]);
                nextText++;
                SEManager.Instance.PlaySE("Click");
            }
            else
            {
                gameScene.allowSceneActivation = true;
            }
            if(nextText == enterZangeScreen +1)
            {
                blackAreaLeft.rectTransform.DOScaleX(1, 3).SetEase(Ease.Linear);
                blackAreaRight.rectTransform.DOScaleX(1, 3).SetEase(Ease.Linear);
                volControl.EnterZangeScreen();
            }
            if(SEDictionary.TryGetValue(nextText-1 ,out var audioClip))
            {
                AudioSource.PlayClipAtPoint(audioClip,Camera.main.transform.position);
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
}

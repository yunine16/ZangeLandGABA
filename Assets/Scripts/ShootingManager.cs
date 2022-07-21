using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShootingManager : MonoBehaviour
{
    int progressPerEnemy = 8;
    int shootingProgress = 0;
    int score = 0;
    int playerLeft = 2;
    int exsistingEnemy=0;
    float level=1;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI leftText;
    [SerializeField] private Image crackImage;
    [SerializeField] private Material crackMat;
    [SerializeField] StageProgressScreen stageProgressScreen;
    [SerializeField] EnemyGenerator enemyGenerator;
    [SerializeField] UIFunctionsForGame uIFunctionsForGame;
    [SerializeField] private ParticleSystem KilledEffectParent,RespawnEffectParent;

    public enum ShootingState
    {
        Start,
        Playing,
        Pause,
        Failure
    }

    public ShootingState shootingState;

    public void ChangeState(ShootingState nextState)
    {
        shootingState = nextState;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartZange());
    }

    public int CheckProgress(int enemyNum)//これ以上敵を作っていいか判断
    {
        return shootingProgress + progressPerEnemy * enemyNum;
    }

    public void Eliminate()
    {
        exsistingEnemy--;
        shootingProgress += progressPerEnemy;
        stageProgressScreen.stageProgress = shootingProgress;
        if (shootingProgress >= 100) { GameCrear(); return; }
        level += 0.4f;
        if (level > 4) level = 4;
        if (exsistingEnemy <= 0) Make();
        Debug.Log("shootingProgress = " + shootingProgress.ToString());
    }

    void Make()
    {
        int num = (int)Random.Range(1, level);
        StartCoroutine(enemyGenerator.Generate(num));
        exsistingEnemy = num;
    }


    IEnumerator StartZange()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(ShootingState.Playing);
        Make();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int point)
    {
        score += point;
        scoreText.text = "Score:" + score.ToString();
    }

    public void AddLeft()
    {
        playerLeft++;
        leftText.text = "x" + playerLeft.ToString();
    }

    public void ReduceLeft()
    {
        if (playerLeft <= 0) { GameOver(); return; }
        playerLeft--;
        leftText.text = "x" + playerLeft.ToString();
    }

    void GameCrear()
    {
        uIFunctionsForGame.Success();
    }

    public void ReStart()
    {
        ChangeState(ShootingState.Playing);
    }

    public void Revenge()
    {
        RespawnEffectParent.Play(true);
        SEManager.Instance.PlaySE("PlayerRespawn");
        Invoke("ReStart",2f);
        for (int i = 0; i < 3; i++)
        {
            AddLeft();
        }
    }

    private void GameOver()
    {
        crackMat.SetInt(crackMat.shader.GetPropertyNameId(1), Random.Range(20, 40));
        crackImage.DOFade(1, 0.1f);
        KilledEffectParent.Play(true);
        SEManager.Instance.PlaySE("PlayerDead");
        ChangeState(ShootingState.Failure);
        uIFunctionsForGame.Invoke("Failure",2f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingManager : MonoBehaviour
{
    int progressPerEnemy = 8;
    int shootingProgress = 0;
    int score = 0;
    int playerLeft = 2;
    int existEnemy=0;
    float level=1;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI leftText;
    [SerializeField] StageProgressScreen stageProgressScreen;
    [SerializeField] EnemyCreater enemyCreater;
    [SerializeField] UIFunctionsForGame uIFunctionsForGame;

    public enum ShootingState
    {
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
        Eliminate();
    }

    public int CheckProgress(int enemyNum)//これ以上敵を作っていいか判断
    {
        return shootingProgress + progressPerEnemy * enemyNum;
    }

    public void Eliminate()
    {
        existEnemy--;
        shootingProgress += progressPerEnemy;
        stageProgressScreen.stageProgress = shootingProgress;
        if (shootingProgress >= 100) GameCrear();
        level += 0.4f;
        if (existEnemy <= 0)
        {
            int num = (int)Random.Range(1, level);
            StartCoroutine(enemyCreater.Create(num));
            existEnemy = num;
        }
        Debug.Log("shootingProgress = " + shootingProgress.ToString());
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
        leftText.text = "×" + playerLeft.ToString();
    }

    public void ReduceLeft()
    {
        if (playerLeft <= 0) { GameOver(); return; }
        playerLeft--;
        leftText.text = "×" + playerLeft.ToString();
    }

    void GameCrear()
    {
        uIFunctionsForGame.Success();
    }

    public void Revenge()
    {
        ChangeState(ShootingState.Playing);
        for (int i = 0; i < 3; i++)
        {
            AddLeft();
        }
    }

    private void GameOver()
    {
        ChangeState(ShootingState.Failure);
        uIFunctionsForGame.Failure();
    }
}

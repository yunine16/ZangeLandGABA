using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingManager : MonoBehaviour
{
    int enemyLeft = 10;
    int score = 0;

    public TextMeshProUGUI scoreText;

    public enum ShootingState
    {
        Playing,
        Pause
    }

    public ShootingState shootingState;

    public void ChangeState(ShootingState nextState)
    {
        shootingState = nextState;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Elminate()
    {
        enemyLeft--;
        Debug.Log("enemyLeft = " + enemyLeft.ToString());
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
}

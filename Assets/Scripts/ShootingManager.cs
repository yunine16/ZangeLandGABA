using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    int enemyLeft = 10;

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
}

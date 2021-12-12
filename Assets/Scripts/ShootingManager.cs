using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

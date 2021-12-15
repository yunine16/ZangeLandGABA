using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonScript : MonoBehaviour
{
    public ShootingManager shootingManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shootingManager.shootingState == ShootingManager.ShootingState.Playing)
            {
                shootingManager.ChangeState(ShootingManager.ShootingState.Pause);
            }else if(shootingManager.shootingState == ShootingManager.ShootingState.Pause)
            {
                shootingManager.ChangeState(ShootingManager.ShootingState.Playing);
            }
        }
    }
}

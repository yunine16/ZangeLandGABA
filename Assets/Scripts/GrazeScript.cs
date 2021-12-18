using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrazeScript : MonoBehaviour
{
    float grazePoint = 0;
    float maxGraze = 1000;
    public Slider grazeGauge;
    [SerializeField] ShootingManager shootingManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            grazePoint += 50;
            if (grazePoint >= maxGraze)
            {
                shootingManager.AddLeft();
                grazePoint = 0;
            }
            grazeGauge.value = grazePoint;
        }
    }
}

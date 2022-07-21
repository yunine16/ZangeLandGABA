using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnemyScript : MonoBehaviour
{
    EnemyGenerator enemyGenerator;
    // Start is called before the first frame update
    void Start()
    {
        enemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator").GetComponent<EnemyGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        enemyGenerator.Effect(transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyDefault : MonoBehaviour
{
    EnemyGenerator enemyGenerator;

    private void Start()
    {
        enemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator").GetComponent<EnemyGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != "Enemy") return;
        if (collision.tag == "Wall") Destroy(gameObject);
    }

    private void OnDestroy()
    {
        enemyGenerator.Effect(transform.position);
    }
}

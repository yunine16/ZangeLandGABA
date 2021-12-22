using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyDefault : MonoBehaviour
{
    EnemyCreater enemyCreater;

    private void Start()
    {
        enemyCreater = GameObject.FindGameObjectWithTag("EnemyCreater").GetComponent<EnemyCreater>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != "Enemy") return;
        if (collision.tag == "Wall") Destroy(gameObject);
    }

    private void OnDestroy()
    {
        enemyCreater.Effect(transform.position);
    }
}

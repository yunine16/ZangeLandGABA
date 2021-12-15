using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != "Enemy") return;
        if (collision.tag == "Wall") Destroy(gameObject);
    }
}

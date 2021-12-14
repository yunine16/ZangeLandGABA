using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletScript : MonoBehaviour
{
    Vector2 vector;
    float speed = 0.3f;
    public ObjectPool<GameObject> objectPool;

    public void SetDirection(Quaternion rot)
    {
        vector = rot * Vector2.up * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            objectPool.Release(gameObject);
        }else if(collision.gameObject.tag == "Wall")
        {
            objectPool.Release(gameObject);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(vector, Space.World);
    }
}

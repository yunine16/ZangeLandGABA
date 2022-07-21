using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletScript : MonoBehaviour
{
    Vector2 vector;
    float speed = 10f;
    public ObjectPool<GameObject> objectPool;
    ShootingManager shootingManager;

    public void SetDirection(Quaternion rot)
    {
        vector = rot * Vector2.up * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        shootingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShootingManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int SEnumber = Random.Range(0, 4);
            SEManager.Instance.PlaySE("BulletHit" + SEnumber);
            Destroy(collision.gameObject);
            shootingManager.AddScore(50);
            objectPool.Release(gameObject);
        }else if(collision.gameObject.tag == "Wall")
        {
            objectPool.Release(gameObject);
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (shootingManager.shootingState != ShootingManager.ShootingState.Playing) return;
        transform.Translate(vector*Time.unscaledDeltaTime, Space.World);
    }
}

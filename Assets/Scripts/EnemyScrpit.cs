using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class EnemyScrpit : MonoBehaviour
{
    Vector3 RotRad = new Vector3(0f, 0f, 1f);
    public ObjectPool<GameObject> objectPool;
    bool isRot,isTrackAttack;
    Vector2 moveVec;
    float speed = 8;
    float rotSpeed = 1440;

    private void Update()
    {
        if (isRot)
        {
            transform.Rotate(RotRad * rotSpeed * Time.deltaTime);
        }
        if (isTrackAttack)
        {
            transform.position += (Vector3)moveVec * speed * Time.deltaTime;
        }
    }

    public void RotStart()
    {
        isRot = true;
    }

    public void Release()
    {
        objectPool.Release(gameObject);
    }

    public void TrackAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        moveVec = (player.transform.position - transform.position).normalized;
        isTrackAttack = true;
        gameObject.tag = "Enemy";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall") Destroy(gameObject);
    }

}
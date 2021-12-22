using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class TileEnemyScript : MonoBehaviour
{
    Vector3 RotRad = new Vector3(0f, 0f, 1f);
    bool isRot,isTrackAttack;
    Vector2 moveVec;
    float speed = 8;
    float rotSpeed = 1440;

    public ObjectPool<GameObject> objectPool;
    EnemyCreater enemyCreater;

    ShootingManager shootingManager;

    Color color;

    private void Start()
    {
        shootingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShootingManager>();
        ColorUtility.TryParseHtmlString("#7B5234", out color);
        enemyCreater = GameObject.FindGameObjectWithTag("EnemyCreater").GetComponent<EnemyCreater>();
    }

    private void Update()
    {
        if (shootingManager.shootingState != ShootingManager.ShootingState.Playing) return;
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

    public void TrackAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        moveVec = (player.transform.position - transform.position).normalized;
        isTrackAttack = true;
        gameObject.tag = "Enemy";
        transform.GetChild(0).GetComponent<TextMesh>().color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrackAttack) return;
        if (collision.tag == "Wall") Destroy(gameObject);
    }

    private void OnDestroy()
    {
        enemyCreater.Effect(transform.position);
    }
}

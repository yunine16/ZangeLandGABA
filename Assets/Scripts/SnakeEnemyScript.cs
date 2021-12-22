using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

public class SnakeEnemyScript : MonoBehaviour
{
    ShootingManager shootingManager;
    Vector3 moveVec;
    bool isAttack;
    float speed = 3;

    public ObjectPool<GameObject> objectPool;
    EnemyCreater enemyCreater;

    Color color;

    // Start is called before the first frame update
    void Start()
    {
        shootingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShootingManager>();
        ColorUtility.TryParseHtmlString("#7B5234", out color);
        enemyCreater = GameObject.FindGameObjectWithTag("EnemyCreater").GetComponent<EnemyCreater>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingManager.shootingState != ShootingManager.ShootingState.Playing) return;
        if (isAttack)
        {
            transform.position += (Vector3)moveVec * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != "Enemy") return;
        if (collision.tag == "Wall") Destroy(gameObject);
    }

    public void SetMoveVec(Vector2 target)
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        moveVec = (target - (Vector2)transform.position).normalized;
        isAttack = true;
    }

    public void AttackStart()
    {
        isAttack = true;
        transform.tag = "Enemy";
        transform.GetChild(0).GetComponent<TextMesh>().color = color;
    }

    private void OnDestroy()
    {
        Debug.Log(transform.position);
        enemyCreater.Effect(transform.position);
    }
}

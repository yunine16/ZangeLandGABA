using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

public class CircleEnemyRoot : MonoBehaviour
{
    public GameObject prefabEnemy;
    public ShootingManager shootingManager;

    Vector2 vector2,moveVec;
    Tweener tweener;
    List<Tweener> childTweens;

    bool isAttack = false;

    float speed=2, rotSpeed=100;
    Vector3 rotRad = new Vector3(0f, 0f, 1f);

    public enum CircleState
    {
        Appear,
        Attack
    }

    CircleState progress;

    // Start is called before the first frame update
    void Start()
    {
        shootingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShootingManager>();
        progress = CircleState.Appear;
        tweener = transform.DOMoveY(transform.position.y -3f, 3f);
        childTweens = new List<Tweener>();
        StartCoroutine(AppearWait());
    }

    public void Init(string zange)
    {
        for (int i = 0; i < zange.Length; i++)
        {
            vector2.x = ((float)i % 10f) / 2f - 2.5f;
            vector2.y = -(float)((int)i / 10)/2;
            GameObject enemy = Instantiate(prefabEnemy, transform);
            enemy.transform.position = (Vector2)transform.position + vector2;
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.GetChild(0).GetComponent<TextMesh>().text = zange[i].ToString();
            enemy.AddComponent<EnemyDefault>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tweener.IsActive())
        {
            if (shootingManager.shootingState == ShootingManager.ShootingState.Pause && tweener.IsPlaying()) tweener.Pause();
            else if (shootingManager.shootingState == ShootingManager.ShootingState.Playing && !tweener.IsPlaying()) tweener.Play();
        }
        foreach (var item in childTweens)
        {
            if (item.IsActive())
            {
                if (shootingManager.shootingState == ShootingManager.ShootingState.Pause && item.IsPlaying()) item.Pause();
                else if (shootingManager.shootingState == ShootingManager.ShootingState.Playing && !item.IsPlaying()) item.Play();
            }
        }
        if (transform.childCount <= 0)
        {
            shootingManager.Eliminate();
            Destroy(this.gameObject);
        }
        if (shootingManager.shootingState != ShootingManager.ShootingState.Playing) return;
        if (isAttack)
        {
            transform.position += (Vector3)moveVec * speed * Time.deltaTime;
            transform.Rotate(rotRad * rotSpeed * Time.deltaTime);
        }
    }

    public IEnumerator CircleAttack()
    {
        int n = transform.childCount;
        float arg = 2 * Mathf.PI / (float)n;
        for (int i = 0; i < n; i++)
        {
            childTweens.Add(transform.GetChild(i).DOMove(transform.position + new Vector3(-2 * Mathf.Cos(arg * i), 2 * Mathf.Sin(arg * i), 0),3f));
            childTweens.Add(transform.GetChild(i).DORotate(new Vector3(0, 0, (float)(90 - 360 / n * i)), 3f));
        }
        yield return StartCoroutine(wait(3f));
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        moveVec = (player.transform.position - transform.position).normalized;
        isAttack = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).tag = "Enemy";
        }
    }


    public IEnumerator AppearWait()
    {
        yield return StartCoroutine(wait(3f));
        progress = CircleState.Attack;
        StartCoroutine(CircleAttack());
    }

    IEnumerator wait(float waitTime)
    {
        float elapsedTime=0f;
        while (waitTime > elapsedTime)
        {
            if (shootingManager.shootingState != ShootingManager.ShootingState.Pause) elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}

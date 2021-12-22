using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

public class SnakeEnemyRoot : MonoBehaviour
{
    public GameObject prefabEnemy;
    public ShootingManager shootingManager;

    Vector3 vector = Vector3.zero, moveVec;
    Tweener tweener;
    List<Vector2> vector2s = new List<Vector2>();
    int snakeLength;
    //bool[] snakeExist;
    GameObject[] snakes;

    GameObject leaderSnake;



    public enum SnakeState
    {
        Appear,
        Attack
    }

    SnakeState progress;

    // Start is called before the first frame update
    void Start()
    {
        shootingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShootingManager>();
        progress = SnakeState.Appear;
        tweener = transform.DOMoveY(transform.position.y - 3f, 3f);
        leaderSnake = Instantiate(new GameObject(), transform);
        leaderSnake.AddComponent<SnakeEnemyScript>();
        StartCoroutine(AppearWait());
    }

    public void Init(string zange)
    {
        snakeLength = zange.Length;
        snakes = new GameObject[snakeLength];
        for (int i = 0; i < zange.Length; i++)
        {
            if (zange[i].ToString() == " " || zange[i].ToString() == "　") continue;
            vector.x = ((float)i % 10f) / 2f - 2.5f;
            vector.y = -(float)((int)i / 10) / 2;
            GameObject enemy = Instantiate(prefabEnemy, transform);
            enemy.transform.position = transform.position + vector;
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.GetChild(0).GetComponent<TextMesh>().text = zange[i].ToString();
            enemy.AddComponent<SnakeEnemyScript>();
            snakes[i] = enemy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tweener.IsActive())
        {
            if (shootingManager.shootingState == ShootingManager.ShootingState.Pause && tweener.IsPlaying()) tweener.Pause();
            else if (shootingManager.shootingState == ShootingManager.ShootingState.Playing && !tweener.IsPlaying()) tweener.Play();
        }/*
        foreach (var item in childTweens)
        {
            if (item.IsActive())
            {
                if (shootingManager.shootingState == ShootingManager.ShootingState.Pause && item.IsPlaying()) item.Pause();
                else if (shootingManager.shootingState == ShootingManager.ShootingState.Playing && !item.IsPlaying()) item.Play();
            }
        }*/
        if (transform.childCount <= 1)
        {
            shootingManager.Eliminate();
            Destroy(this.gameObject);
        }
        if (shootingManager.shootingState != ShootingManager.ShootingState.Playing) return;
        /*if (isAttack)
        {
            transform.position += moveVec * speed * Time.deltaTime;
            transform.Rotate(rotRad * rotSpeed * Time.deltaTime);
        }*/
    }

    public IEnumerator SnakeAttack()
    {
        int trackLimit = 10;
        int moveSnake = 0;
        while (transform.childCount > 1)
        {
            if (moveSnake < snakeLength) moveSnake++;
            if (trackLimit > 0)
            {
                Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                leaderSnake.GetComponent<SnakeEnemyScript>().SetMoveVec(player.transform.position);
                trackLimit--;
            }
            //目的地の設定
            vector2s.Insert(0, (Vector2)leaderSnake.transform.position);
            if (vector2s.Count > snakeLength) vector2s.RemoveAt(vector2s.Count - 1);
            if (snakes[moveSnake-1] != null) snakes[moveSnake-1].GetComponent<SnakeEnemyScript>().AttackStart();
            for (int i = 0; i < moveSnake; i++)
            {
                if (snakes[i] == null) continue;
                snakes[i].GetComponent<SnakeEnemyScript>().SetMoveVec(vector2s[i]);
            }
            yield return StartCoroutine(wait(0.2f));
        }

    }


    public IEnumerator AppearWait()
    {
        yield return StartCoroutine(wait(3f));
        progress = SnakeState.Attack;
        StartCoroutine(SnakeAttack());
    }

    IEnumerator wait(float waitTime)
    {
        float elapsedTime = 0f;
        while (waitTime > elapsedTime)
        {
            if (shootingManager.shootingState == ShootingManager.ShootingState.Playing) elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DG.Tweening;

public class TileEnemyRoot : MonoBehaviour
{
    public GameObject prefabEnemy;
    public ShootingManager shootingManager;

    Vector3 vector = Vector3.zero;

    Tweener tweener;
    bool isTweenMove;

    public enum TileState
    {
        Appear,
        Attack
    }

    TileState progress;

    // Start is called before the first frame update
    void Start()
    {
        shootingManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShootingManager>();
        progress = TileState.Appear;
        tweener = transform.DOMoveY(transform.position.y -3f, 3f);
        StartCoroutine(AppearWait());
    }

    public void Init(string zange)
    {
        for (int i = 0; i < zange.Length; i++)
        {
            vector.x = ((float)i % 10f)/2f;
            vector.y = -(float)((int)i / 10)/2;
            GameObject enemy = Instantiate(prefabEnemy, transform);
            enemy.AddComponent<TileEnemyScript>();
            enemy.transform.position = transform.position + vector;
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.GetChild(0).GetComponent<TextMesh>().text = zange[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(progress == TileState.Appear)
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
        */
        if (tweener.IsActive())
        {
            if (shootingManager.shootingState != ShootingManager.ShootingState.Playing && tweener.IsPlaying()) tweener.Pause();
            else if (shootingManager.shootingState == ShootingManager.ShootingState.Playing && !tweener.IsPlaying()) tweener.Play();
        }
        if (transform.childCount <= 0)
        {
            shootingManager.Eliminate();
            Destroy(this.gameObject);
        }
    }

    public IEnumerator TileAttack()
    {
        // 子オブジェクトを返却する配列作成
        var children = new Transform[transform.childCount];

        // 0～個数-1までの子を順番に配列に格納
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = transform.GetChild(i);
        }

        children = children.OrderBy(a => Guid.NewGuid()).ToArray<Transform>();

        foreach (Transform item in children)
        {
            yield return StartCoroutine(wait(0.5f));
            StartCoroutine(TileRot(item));
            yield return StartCoroutine(wait(0.5f));
        }
        yield return null;
    }

    IEnumerator TileRot(Transform item)
    {
        TileEnemyScript enemyScript = item.GetComponent<TileEnemyScript>();
        enemyScript.RotStart();
        yield return StartCoroutine(wait(2f));
        enemyScript.TrackAttack();
    }

    public IEnumerator AppearWait()
    {
        yield return StartCoroutine(wait(3f));
        progress = TileState.Attack;
        StartCoroutine(TileAttack());
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

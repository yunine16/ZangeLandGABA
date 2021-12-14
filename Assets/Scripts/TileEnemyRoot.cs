using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TileEnemyRoot : MonoBehaviour
{
    public GameObject prefabEnemy;
    public ShootingManager shootingManager;

    Vector2 vector2;

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
        StartCoroutine(AppearWait());
    }

    public void Init(string zange)
    {
        for (int i = 0; i < zange.Length; i++)
        {
            vector2.x = ((float)i % 10f)/2f;
            vector2.y = -(float)((int)i / 10)/2;
            GameObject enemy = Instantiate(prefabEnemy, transform);
            enemy.transform.position = (Vector2)transform.position + vector2;
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.GetChild(0).GetComponent<TextMesh>().text = zange[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(progress == TileState.Appear)
        {
            transform.position += Vector3.down * Time.deltaTime;
        }
        if (transform.childCount <= 0)
        {
            shootingManager.Elminate();
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
            yield return StartCoroutine(wait(1f));
            StartCoroutine(TileRot(item));
            yield return StartCoroutine(wait(1f));
        }
        yield return null;
    }

    IEnumerator TileRot(Transform item)
    {
        EnemyScrpit enemyScript = item.GetComponent<EnemyScrpit>();
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

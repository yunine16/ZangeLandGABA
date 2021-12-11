using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TileEnemyRoot : MonoBehaviour
{
    public GameObject prefabEnemy;

    Vector2 vector2;

    public enum EnemyType
    {
        Tile,
    }

    bool isactive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(string zange)
    {
        for (int i = 0; i < zange.Length; i++)
        {
            vector2.x = i % 10;
            vector2.y = -(int)i / 10;
            GameObject enemy = Instantiate(prefabEnemy, transform);
            enemy.transform.position = vector2;
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.GetChild(0).GetComponent<TextMesh>().text = zange[i].ToString();
        }
        StartCoroutine(TileAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            yield return new WaitForSeconds(1f);
            StartCoroutine(TileRot(item));
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    IEnumerator TileRot(Transform item)
    {
        EnemyScrpit enemyScript = item.GetComponent<EnemyScrpit>();
        enemyScript.RotStart();
        yield return new WaitForSeconds(2f);
        enemyScript.TrackAttack();
    }

}

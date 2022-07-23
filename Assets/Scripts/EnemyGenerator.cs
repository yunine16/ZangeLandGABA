using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Pool;

public class EnemyGenerator : MonoBehaviour
{

    string zange = "改行コード変更してコンフリクト";
    [SerializeField]
    GameObject prefabEnemy;
    Vector2[] vector2 = { new Vector2(0, -1.5f), new Vector2(-3, 0), new Vector2(3, 0), new Vector2(-3, -1.5f), new Vector2(3, -1.5f) };
    List<int[]> indexes = new List<int[]>();
    public GameObject TileEnemyRoot,CircleEnemyRoot,SnakeEnemyRoot;
    [SerializeField] ShootingManager shootingManager;
    GetRandomZange getRandomZange;

    ObjectPool<GameObject> objectPool;
    [SerializeField]GameObject prefabHitEffect;


    enum EnemyType
    {
        Tile = 0,
        Circle = 1,
        Snake = 2
        //HourGlass = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        indexes.Add(new int[] { 0, 1, 2, 3 });
        indexes.Add(new int[] { 1, 2 });
        indexes.Add(new int[] { 0, 1, 2});
        indexes.Add(new int[] { 1, 2, 3, 4 });
        getRandomZange = GetComponent<GetRandomZange>();
        //zange = getRandomZange.GetZange();

        // オブジェクトプールを作成
        objectPool = new ObjectPool<GameObject>(() =>
        {
            // 生成処理
            var effect = Instantiate(prefabHitEffect);
            var pooled = effect.GetComponent<EnemyHitEffect>();
            pooled.objectPool = objectPool;
            return effect;
        },
        target =>
        {
            // 再利用処理
            target.SetActive(true);
        },
        target =>
        {
            // プールに戻す処理
            target.SetActive(false);
        },
        target =>
        {
            // プールの許容量を超えた場合の破棄処理
            Destroy(target);
        }, true, 100, 1000);
    }

    public void Effect(Vector3 vector3)
    {
        GameObject effect = objectPool.Get();
        effect.transform.position = vector3;
        effect.transform.rotation = Quaternion.identity;
        effect.GetComponent<EnemyHitEffect>().Play();
    }

    public IEnumerator Generate(int num)
    {
        int[] arr = indexes[num - 1].OrderBy(i => Guid.NewGuid()).ToArray<int>();
        for (int i = 0; i < num; i++)
        {
            if (shootingManager.CheckProgress(i) >= 100) yield break;
            EnemyType enemyType = (EnemyType)(int)UnityEngine.Random.Range(0, 3);
            Vector2 vec = vector2[arr[i]];
            zange = getRandomZange.GetZange();
            switch (enemyType)
            {
                case EnemyType.Tile:
                    Instantiate(TileEnemyRoot, transform.position + (Vector3)vec, Quaternion.identity).GetComponent<TileEnemyRoot>().Init(zange);
                    break;
                case EnemyType.Circle:
                    Instantiate(CircleEnemyRoot, transform.position + (Vector3)vec, Quaternion.identity).GetComponent<CircleEnemyRoot>().Init(zange);
                    break;
                case EnemyType.Snake:
                    Instantiate(SnakeEnemyRoot, transform.position + (Vector3)vec, Quaternion.identity).GetComponent<SnakeEnemyRoot>().Init(zange);
                    break;
                default:
                    break;
            }
            yield return StartCoroutine(wait(1f));
        }
        
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


    // Update is called once per frame
    void Update()
    {
        
    }
}

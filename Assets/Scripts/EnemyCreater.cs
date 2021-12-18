using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyCreater : MonoBehaviour
{

    string zange = "脳死で改行コード変更してコンフリクト";
    [SerializeField]
    GameObject prefabEnemy;
    Vector2 vector2;
    public GameObject TileEnemyRoot,CircleEnemyRoot;
    [SerializeField] ShootingManager shootingManager;

    enum EnemyType
    {
        Tile = 0,
        Circle = 1,
        Snake = 2,
        HourGlass = 3
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator Create(int num)
    {
        for (int i = 0; i <num; i++)
        {
            if (shootingManager.CheckProgress(i) >= 100) yield break;
            EnemyType enemyType = (EnemyType)(int)Random.Range(0, 2);
            switch (enemyType)
            {
                case EnemyType.Tile:
                    Instantiate(TileEnemyRoot, transform.position, Quaternion.identity).GetComponent<TileEnemyRoot>().Init(zange);
                    break;
                case EnemyType.Circle:
                    Instantiate(CircleEnemyRoot, transform.position, Quaternion.identity).GetComponent<CircleEnemyRoot>().Init(zange);
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

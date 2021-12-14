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
    public GameObject TileEnemyRoot;
    ObjectPool<GameObject> objectPool;

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
        EnemyType enemyType = (EnemyType)0;

        switch (enemyType)
        {
            case EnemyType.Tile:
                Instantiate(TileEnemyRoot,transform.position,Quaternion.identity).GetComponent<TileEnemyRoot>().Init(zange);
                break;
            default:
                break;
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

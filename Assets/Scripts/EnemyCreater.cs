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

    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(EnemyRoot.GetComponent<TileEnemyRoot>().TileAttack());
        Instantiate(TileEnemyRoot).GetComponent<TileEnemyRoot>().Init(zange);

        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

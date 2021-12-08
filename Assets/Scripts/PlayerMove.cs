using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMove : MonoBehaviour
{
    Vector2 vector;
    float speed = 0.2f;
    float shotCoolDown = 0.2f;
    public GameObject prefabBullet;
    float elapsedTime;
    ObjectPool<GameObject> objectPool;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトプールを作成
        objectPool = new ObjectPool<GameObject>(() =>
        {
            // 生成処理
            var bullet = Instantiate(prefabBullet);
            var pooled = bullet.AddComponent<BulletScript>();
            pooled.objectPool = objectPool;
            return bullet;
        },
        target =>
        {
            // 再利用処理
            print("GET");
            target.SetActive(true);
        },
        target =>
        {
            // プールに戻す処理
            print("RELEASE");
            target.SetActive(false);
        },
        target =>
        {
            // プールの許容量を超えた場合の破棄処理
            print("DESTROY");
            Destroy(target);
        }, true, 100, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        //WASDで移動
        vector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) vector += Vector2.up * speed;
        if (Input.GetKey(KeyCode.A)) vector += Vector2.left * speed;
        if (Input.GetKey(KeyCode.S)) vector += Vector2.down * speed;
        if (Input.GetKey(KeyCode.D)) vector += Vector2.right * speed;

        gameObject.transform.Translate(vector,Space.World);

        //カーソルの方向を向く
        var pos = Camera.main.WorldToScreenPoint(transform.localPosition);
        var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
        transform.localRotation = rotation;

        //クリックで発射
        if (Input.GetMouseButton(0) && elapsedTime >= shotCoolDown)
        {
            Shot(rotation);
            elapsedTime = 0;
        }
        if (elapsedTime < shotCoolDown) elapsedTime += Time.deltaTime;
    }

    void Shot(Quaternion rot)
    {
        GameObject bullet = objectPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = rot;
        //GameObject bullet = Instantiate(prefabBullet, transform.position, rot, null);
        bullet.GetComponent<BulletScript>().SetDirection(rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log("EnemyにPlayerがHIT!");
        }
    }
}

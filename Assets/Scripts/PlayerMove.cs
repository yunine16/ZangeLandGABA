using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    Vector2 vector;
    public float speed = 0.2f;
    float shotCoolDown = 0.2f;
    public GameObject prefabBullet;
    float elapsedTime;
    ObjectPool<GameObject> objectPool;
    [SerializeField]
    GameObject player;

    bool isInvincible;

    public ShootingManager shootingManager;
    Renderer[] shipRenderers;
    int left=2;

    // Start is called before the first frame update
    void Start()
    {
        shipRenderers = GetComponentsInChildren<Renderer>();

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

    // Update is called once per frame
    void Update()
    {
        if (shootingManager.shootingState != ShootingManager.ShootingState.Playing) return;
        //WASDで移動
        vector = Vector2.zero;
        if (Input.GetKey(KeyCode.W) && transform.position.y < 2.75f) vector += Vector2.up * speed;
        if (Input.GetKey(KeyCode.A) && transform.position.x > -6f) vector += Vector2.left * speed;
        if (Input.GetKey(KeyCode.S) && transform.position.y > -4.45f) vector += Vector2.down * speed;
        if (Input.GetKey(KeyCode.D) && transform.position.x < 6f) vector += Vector2.right * speed;

        player.transform.Translate(vector,Space.World);

        //カーソルの方向を向く
        var pos = Camera.main.WorldToScreenPoint(player.transform.localPosition);
        var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
        transform.localRotation = rotation;

        //クリックで発射
        if (Input.GetMouseButton(0) && elapsedTime >= shotCoolDown)
        {
            elapsedTime = 0;
            Shot(rotation);
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
        string typeSENumber = Random.Range(0, 4).ToString();
        SEManager.Instance.PlaySE("Typing" + typeSENumber);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !isInvincible)
        {
            shootingManager.ReduceLeft();
            isInvincible = true;
            StartCoroutine(Invincible());
        }
    }

    IEnumerator Invincible()
    {
        for (int i = 0; i < 4; i++)
        {
            shipRenderers[0].enabled = false;
            shipRenderers[1].enabled = false;
            yield return StartCoroutine(wait(0.125f));
            shipRenderers[0].enabled = true;
            shipRenderers[1].enabled = true;
            yield return StartCoroutine(wait(0.125f));
        }
        isInvincible = false;
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

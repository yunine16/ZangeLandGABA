using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector2 vector;
    float speed = 0.2f;
    float shotCoolDown = 0.2f;
    public GameObject prefabBullet;
    float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        
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
        GameObject bullet = Instantiate(prefabBullet, transform.position, rot, null);
        bullet.GetComponent<BulletScript>().SetDirection(rot);
    }
}

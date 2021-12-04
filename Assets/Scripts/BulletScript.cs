using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector2 vector;
    float speed = 0.3f;

    public void SetDirection(Quaternion rot)
    {
        vector = rot * Vector2.up * speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        transform.Translate(vector, Space.World);
    }
}

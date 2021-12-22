using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitEffect : MonoBehaviour
{
    ParticleSystem particleSystem;
    public ObjectPool<GameObject> objectPool;
    bool playing;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem.isStopped && playing) { playing = false; objectPool.Release(gameObject); }
    }

    public void Play()
    {
        //particleSystem.Play(true);
        playing = true;
    }


}

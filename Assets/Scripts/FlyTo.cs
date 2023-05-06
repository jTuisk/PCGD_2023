using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTo : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 target;
    Vector3 startPos;
    public Transform shakeTarget;
    public float flytime = 1f;
    float t = 0;
    void Start()
    {
        startPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        transform.position=Vector3.Lerp(startPos, target, t / flytime);
        if (t / flytime > 1)
        {
            CameraEffectManager._instance.ObjectShake(shakeTarget, 0.5f, 1f);
            Destroy(gameObject);
        }
    }
}

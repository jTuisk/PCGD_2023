using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanAfterPlaying : MonoBehaviour
{
    // Start is called before the first frame update
    public float time = 3f;
    void Start()
    {
        Destroy(this.gameObject,time);
    }

    // Update is called once per frame

}

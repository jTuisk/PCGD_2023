using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    private Vector3 pos_1;
    private Vector3 pos_2;
    public float incrementY = 5f;
    public float speed = 7f;
    private bool isDirectionToPos1 = false;
    // Start is called before the first frame update
    void Start()
    {
        pos_1 = transform.position;
        pos_2 = new Vector3(transform.position.x, transform.position.y + incrementY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDirectionToPos1)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos_1, speed * Time.deltaTime);
            if(transform.position == pos_1)
                isDirectionToPos1 = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos_2, speed * Time.deltaTime);
            if(transform.position == pos_2)
                isDirectionToPos1 = true;
        }        
    }
}

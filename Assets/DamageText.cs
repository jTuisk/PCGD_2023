using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI text;
    public float speed=1;
    public float length = 3;
    public void die(){
        Destroy(gameObject);

    }
    void Start()
    {
        Invoke("die",length);
        //text.transform.position+=new Vector3(Random.Range(-3f,3f),0,0);
    }
    public void changeTextString(string tex){
        text.text=tex;

    }
    float time=0;
    void Update()
    {
        text.alpha=Mathf.Lerp(1,0,time/length);
        text.transform.position+=Vector3.up*speed*Time.deltaTime;
        time+=Time.deltaTime;
    }
}

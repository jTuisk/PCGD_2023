using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshProUGUI text;
    public void die(){
        Destroy(gameObject);

    }
    void Start()
    {
        Invoke("die",3);
    }
    public void changeTextString(string tex){
        text.text=tex;

    }

    // Update is called once per frame
    float time=0;
    void Update()
    {
        text.alpha=Mathf.Lerp(1,0,time/3);
        time+=Time.deltaTime;
    }
}

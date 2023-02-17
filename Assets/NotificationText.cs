using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMesh;
    private float lifeTime = 3.0f;
    private float freezeTime = 2.0f;

    public void die(){
        Destroy(gameObject);

    }
    void Start()
    {
        Invoke("die", lifeTime);
    }
    
    public void SetText(string str)
    {
        textMesh.text = str;
    }

    
    float time=0;
    void Update()
    {
        if(time >= freezeTime)
        {
            textMesh.alpha=Mathf.Lerp(1,0,(time - freezeTime)/(lifeTime - freezeTime));
        }
        time += Time.deltaTime;
    }
}

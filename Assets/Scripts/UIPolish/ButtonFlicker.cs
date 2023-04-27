using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    Image image;
    protected float timer;
    protected float t;
    public float flickerSpeed;
    public Color normalColor;
    public Color flickerColor;
    public bool Flickering=true;
    public Vector3 maxScale=new Vector3(1.3f,1.3f,1.3f);
    private Vector3 minScale;
    void Start()
    {
        image = GetComponent<Image>(); // get the image component
        timer = 0f; // reset the timer
        minScale = transform.localScale;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Flickering)
        {
            timer += Time.deltaTime * flickerSpeed; // increase the timer by delta time times flicker speed
            t = Mathf.Abs(Mathf.Sin(timer)); // get a value between 0 and 1 based on the sine of the timer
            image.color = Color.Lerp(normalColor, flickerColor, t); // interpolate between normal and flicker colors based on t
            transform.localScale = Vector3.Lerp(minScale, maxScale, t);
        }
        else
        {
            transform.localScale = minScale;
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    Image image;
    float timer;
    public float flickerSpeed;
    public Color normalColor;
    public Color flickerColor;
    public bool Flickering=true;
    void Start()
    {
        image = GetComponent<Image>(); // get the image component
        timer = 0f; // reset the timer
    }

    // Update is called once per frame
    void Update()
    {
        if (Flickering)
        {
            timer += Time.deltaTime * flickerSpeed; // increase the timer by delta time times flicker speed
            float t = Mathf.Abs(Mathf.Sin(timer)); // get a value between 0 and 1 based on the sine of the timer
            image.color = Color.Lerp(normalColor, flickerColor, t); // interpolate between normal and flicker colors based on t
        }
    }
}

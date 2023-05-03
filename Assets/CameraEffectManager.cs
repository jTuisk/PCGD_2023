using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectManager : MonoBehaviour
{
    public static CameraEffectManager _instance;



    private void Awake()
    {
        // Make sure there is only one instance
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
            _instance = this;
        }
        else
        {
            _instance = this;
            // Optionally, prevent the object from being destroyed when changing scenes
            //DontDestroyOnLoad(gameObject);
        }
    }
    public void shake()
    {
        StartCoroutine(ScreenShake(0.2f, 0.1f));

    }
IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        Quaternion originalRot = transform.rotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Generate a random offset
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            // Apply the offset to the camera position
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + z);

            // Generate a random angle
            float angle = Random.Range(-1f, 1f) * magnitude;

            // Apply the angle to the camera rotation
            transform.rotation = Quaternion.Euler(originalRot.eulerAngles + new Vector3(0f, 0f, angle));

            // Update the elapsed time
            elapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Restore the original position and rotation of the camera
        transform.position = originalPos;
        transform.rotation = originalRot;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

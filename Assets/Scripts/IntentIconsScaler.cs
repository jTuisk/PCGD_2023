using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntentIconsScaler : MonoBehaviour
{
    [SerializeField] bool autoScale = true;

    [Header("Icons")]
    [SerializeField, Range(0.1f, 2f)] float iconSizeMultiplier = 1f;
    [SerializeField] List<RectTransform> iconsRectTransforms;
    
    List<Vector2> defaultSizes;


    private void Awake()
    {
        defaultSizes = GetDefaultSize();
    }

    private void Update()
    {
        if (autoScale)
        {
            ResizeIcons();
        }
    }

    private List<Vector2> GetDefaultSize()
    {
        List<Vector2> sizes = new List<Vector2>();

        foreach(RectTransform RT in iconsRectTransforms)
        {
            sizes.Add(RT.sizeDelta);
        }

        return sizes;
    }

    private void ResizeIcons()
    {
        if (iconsRectTransforms.Count == defaultSizes.Count)
        {
            for (int i = 0; i < iconsRectTransforms.Count; i++)
            {
                iconsRectTransforms[i].sizeDelta = CalculateSize(defaultSizes[i], GetActivatedIconsCount());
            }
        }
    }

    private int GetActivatedIconsCount()
    {
        int count = 0;

        foreach(RectTransform RT in iconsRectTransforms)
        {
            if(RT.gameObject.activeSelf)
                count++;
        }
        return count;
    }

    private Vector2 CalculateSize(Vector2 orginalSize, int currentlyActivated)
    {
        if (currentlyActivated < 1)
            currentlyActivated = 1;

        float sizeMultiplier = Mathf.Min(1f, 1f / (float)currentlyActivated * 2f);

        return orginalSize * sizeMultiplier * iconSizeMultiplier;
    }
}

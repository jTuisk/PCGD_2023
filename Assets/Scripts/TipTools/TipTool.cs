using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TipTool : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameObject tipToolPrefab;
    [TextArea(3,10)]
    [SerializeField] private string displayText;
    protected GameObject tipToolObj;
    protected Camera topCamera;

    protected Vector2 toolMaxSize;
    protected RectTransform toolMaxRect;

    [HideInInspector] public bool canShow;

    protected void FindRenderingCamera()
    {
        foreach(var camera in Camera.allCameras)
        {
            if(camera.name == "TopCamera")
            {
                topCamera = camera;
            }
        }
    }

    #region calibration-related
    
    // Calculate maximum rect size of a gameObject
    protected Vector2 GetMaxRectSize(GameObject gameObject)
    {
        if(toolMaxSize == Vector2.zero)
        {
            foreach(var rect in gameObject.GetComponentsInChildren<RectTransform>())
            {
                var tmpSize = new Vector2(rect.sizeDelta.x * rect.localScale.x,
                    rect.sizeDelta.y * rect.localScale.y);

                toolMaxSize = new Vector2((tmpSize.x > toolMaxSize.x)?tmpSize.x:toolMaxSize.x,
                    (tmpSize.y > toolMaxSize.y)?tmpSize.y:toolMaxSize.y);
            }
        }
        return toolMaxSize;
    }

    // Return the max RectTransform under a gameObject
    protected RectTransform GetMaxRectTransform(GameObject gameObject)
    {
        if(toolMaxRect != null)
            return toolMaxRect;

        var rectTransforms = gameObject.GetComponentsInChildren<RectTransform>();
        if(rectTransforms.Length <= 0)
            return null;
        
        Vector2 maxSize = Vector2.zero;

        foreach(var rect in rectTransforms)
        {
            if(rect.GetComponent<Canvas>() != null)
                continue;

            var tmpSize = new Vector2(rect.sizeDelta.x * rect.localScale.x,
                rect.sizeDelta.y * rect.localScale.y);

            if(maxSize.x <= tmpSize.x && maxSize.y <= tmpSize.y)
            {
                toolMaxRect = rect;
                maxSize = tmpSize;
            }
        }
        return toolMaxRect;
    }

    protected virtual Vector2 GetClibrationOffset(Vector2 toolSize)
    {
        // TODO check if it's full screen or not, cursor size need slight change
        Vector2 cursorSize = new Vector2(20f, 20f);
        return new Vector2(toolSize.x/2 + cursorSize.x, toolSize.y/2 + cursorSize.y);
    }

    // Only Calibrate the panel's position under its parent Canvas
    protected virtual void CalibratePositionUnderWorldPoint(Camera cam, GameObject gameObject)
    {
        float margin = 23f;
        var topRightCornor = new Vector2(Screen.width - margin, Screen.height - margin);
        var bottomRightCornor = new Vector2(Screen.width - margin, 0f);

        var rectTransform = gameObject.GetComponent<RectTransform>();
        var offset = GetClibrationOffset(rectTransform.sizeDelta);

        var currentPos = gameObject.transform.position;
        var calibratedPos = new Vector2(currentPos.x + offset.x, currentPos.y - offset.y);

        if((currentPos.x + rectTransform.sizeDelta.x)> topRightCornor.x &&
        (currentPos.y - rectTransform.sizeDelta.y)< bottomRightCornor.y)
        {
            calibratedPos = new Vector2(
                calibratedPos.x - offset.x * 2,
                calibratedPos.y + offset.y * 2);
        }
        else
        {
            calibratedPos = new Vector2(
                ((currentPos.x + rectTransform.sizeDelta.x)> topRightCornor.x)?
                (topRightCornor.x - rectTransform.sizeDelta.x + offset.x):calibratedPos.x,
                ((currentPos.y - rectTransform.sizeDelta.y)< bottomRightCornor.y)?
                (bottomRightCornor.y + rectTransform.sizeDelta.y - offset.y):calibratedPos.y);
        }

        gameObject.transform.position = new Vector3(calibratedPos.x, calibratedPos.y, gameObject.transform.position.z);
    }

    #endregion


    void Start()
    {
        tipToolObj = Instantiate(tipToolPrefab);
        var textMesh = tipToolObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        textMesh.text = displayText;
        
        FindRenderingCamera();
        DisplayOnPinterExit(null);
    }

    protected void Update()
    {
        UpdateTipToolDisplay();
    }

    protected virtual void UpdateTipToolDisplay()
    {
        if(tipToolObj.activeInHierarchy == true)
        {
            var tipToolPanelUnderCanvas = tipToolObj.GetComponentInChildren<TipToolSizeFitter>();
            if(tipToolPanelUnderCanvas != null)
            {
                tipToolPanelUnderCanvas.transform.position = 
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

                CalibratePositionUnderWorldPoint(topCamera, tipToolPanelUnderCanvas.gameObject);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(canShow)
            DisplayOnPinterEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisplayOnPinterExit(eventData);
    }

    protected virtual void DisplayOnPinterEnter(PointerEventData eventData)
    {
        if(tipToolObj != null)
            tipToolObj.SetActive(true);
    }

    protected virtual void DisplayOnPinterExit(PointerEventData eventData)
    {
        if(tipToolObj != null)
        {
            tipToolObj.SetActive(false);
            tipToolObj.transform.position = new Vector3(2000, 2000, 0);
        }
    }

    void OnEnable()
    {
        canShow = true;
    }

    void OnDisable()
    {
        canShow = false;
        DisplayOnPinterExit(null);
    }
}

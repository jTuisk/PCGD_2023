using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleCardTipTool : TipTool
{
    public BattleCardDataContainer battleCardData;

    private Vector2 leftBottomAnchor;
    private Vector2 rightTopAnchor;

    void Start()
    {
        if(battleCardData == null)
            return;

        // Using Battle Card prefab
        tipToolPrefab = Deck.Instance.CardBasePrefab;
        tipToolObj = Instantiate(tipToolPrefab);
        tipToolObj.name = "test";
        tipToolObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        // Remove unnecessary components, e.g., Card
        var card = tipToolObj.GetComponentInChildren<Card>();
        card.createCard(battleCardData, false); // false?
        Destroy(card);

        # region Rendering
        // Set Layer as TipTop, Top camera render them
        tipToolObj.layer = 6;
        var transforms = tipToolObj.GetComponentsInChildren<Transform>();
        foreach(var transform in transforms)
        {
            transform.gameObject.layer = 6;
        }

        var battleCardCanvas = tipToolObj.GetComponentInChildren<Canvas>();
        battleCardCanvas.sortingOrder=4000;
        
        FindRenderingCamera();
        #endregion

        # region Disable Raycast
        // Disable all raycast target, so that it won't block any rays
        var raycasters = tipToolObj.GetComponentsInChildren<GraphicRaycaster>();
        for(int i = raycasters.Length - 1; i >=0; i --)
        {
            Destroy(raycasters[i]);
        }

        var images = tipToolObj.GetComponentsInChildren<Image>();
        foreach(var img in images)
        {
            img.raycastTarget = false;
        }
        
        var textMeshPros = tipToolObj.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        foreach(var text in textMeshPros)
        {
            text.raycastTarget = false;
        }

        # endregion
        // Set Inactive
        DisplayOnPinterExit(null);
    }
    
    new void Update()
    {
        CalculateBorder();
        if(CanShow())
        {
            tipToolObj.SetActive(true);
        }
        else
        {
            tipToolObj.SetActive(false);
        }
        UpdateTipToolDisplay();
    }

    protected override void UpdateTipToolDisplay()
    {
        // TODO check if it's full screen or not, cursor size need slight change
        Vector2 cursorSize = new Vector2(20f, 20f);

        if(tipToolObj.activeInHierarchy == true)
        {
            var campos = topCamera.ScreenToWorldPoint(Input.mousePosition);

            var mousepos = new Vector3(campos.x , campos.y , 0f);
            tipToolObj.transform.position = mousepos;

            CalibratePositionUnderWorldPoint(topCamera, tipToolObj);
        }
    }

    protected override Vector2 GetClibrationOffset(Vector2 toolSize)
    {
        return new Vector2(toolSize.x / 2 + 0.1f, toolSize.x / 2 + 0.1f);
    }

    // Calibrate the position of a gameObject so that with current size,
    // The camera can still display the entire gameObject
    protected override void CalibratePositionUnderWorldPoint(Camera cam, GameObject gameObject)
    {
        var topRightCornor = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        var bottomRightCornor = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
        var topLeftCornor = cam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f));
        var bottomLeftCornor = cam.ScreenToWorldPoint(Vector3.zero);
        
        var tipToolRect = GetMaxRectTransform(gameObject);
        Vector3[] objectCorners = new Vector3[4];
        tipToolRect.GetWorldCorners(objectCorners);

        Vector2 tipToolSizeToWorldPoint = new Vector2(
            objectCorners[2].x - objectCorners[0].x,
            objectCorners[2].y - objectCorners[0].y);

        // add offset
        var offset = GetClibrationOffset(tipToolSizeToWorldPoint);

        var currentPos = gameObject.transform.position;
        var calibratedPos = new Vector2(currentPos.x + offset.x, currentPos.y - offset.y);
        
        // If both side exceed the border, align the bottom-right corner of tiptool with mousePosition
        if((currentPos.x + tipToolSizeToWorldPoint.x)> topRightCornor.x &&
        (currentPos.y - tipToolSizeToWorldPoint.y)< bottomRightCornor.y)
        {
            calibratedPos = new Vector2(
                calibratedPos.x - tipToolSizeToWorldPoint.x/2 - offset.x,
                calibratedPos.y + tipToolSizeToWorldPoint.y/2 + offset.y);
        }
        else
        {
            calibratedPos = new Vector2(
                ((currentPos.x + tipToolSizeToWorldPoint.x)> topRightCornor.x)?
                (topRightCornor.x - tipToolSizeToWorldPoint.x + offset.x):calibratedPos.x,
                ((currentPos.y - tipToolSizeToWorldPoint.y)< bottomRightCornor.y)?
                (bottomRightCornor.y + tipToolSizeToWorldPoint.y - offset.y):calibratedPos.y);
        }
        
        gameObject.transform.position = new Vector3(calibratedPos.x, calibratedPos.y, gameObject.transform.position.z);
    }

    // Somehow, these functions have display error.
    protected override void DisplayOnPinterEnter(PointerEventData eventData)
    {
        // tipToolObj.SetActive(true);
    }

    protected override void DisplayOnPinterExit(PointerEventData eventData)
    {
        // tipToolObj.SetActive(false);
        // tipToolObj.transform.position = new Vector3(2000, 2000, 0);
    }

    private bool CanShow()
    {
        if(!canShow)
            return false;

        return (
            Input.mousePosition.x > leftBottomAnchor.x && 
            Input.mousePosition.x < rightTopAnchor.x &&
            Input.mousePosition.y > leftBottomAnchor.y &&
            Input.mousePosition.y < rightTopAnchor.y
        );
    }
    
    private void CalculateBorder()
    {
        float margin = 3f;

        var screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        var rect = this.GetComponent<RectTransform>().rect;

        leftBottomAnchor = new Vector2(
            screenPos.x + margin,
            screenPos.y + margin
        );

        rightTopAnchor = new Vector2(
            screenPos.x + rect.width - margin,
            screenPos.y + rect.height - margin
        );
    }


    void OnDestroy()
    {
        Destroy(tipToolObj);
    }
}

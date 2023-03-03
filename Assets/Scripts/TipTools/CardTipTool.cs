using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleCardTipTool : TipTool
{
    public BattleCardDataContainer battleCardData;
    private Camera topCamera;

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
        
        foreach(var camera in Camera.allCameras)
        {
            if(camera.name == "TopCamera")
            {
                topCamera = camera;
            }
        }
        #endregion

        // Set Inactive
        DisplayOnPinterExit(null);
    }
    
    void Update()
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
            var mousepos = new Vector3(campos.x + 1.5f, campos.y - 1.5f, 0f);
            tipToolObj.transform.position = mousepos;                    
        }
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

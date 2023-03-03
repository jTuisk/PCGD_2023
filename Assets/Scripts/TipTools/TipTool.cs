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


    void Start()
    {
        tipToolObj = Instantiate(tipToolPrefab);
        var textMesh = tipToolObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        textMesh.text = displayText;
        
        DisplayOnPinterExit(null);
    }

    protected void Update()
    {
        UpdateTipToolDisplay();
    }

    protected virtual void UpdateTipToolDisplay()
    {
        // TODO check if it's full screen or not, cursor size need slight change
        Vector2 cursorSize = new Vector2(20f, 20f);

        if(tipToolObj.activeInHierarchy == true)
        {
            var tipToolPanelUnderCanvas = tipToolObj.GetComponentInChildren<TipToolSizeFitter>();
            if(tipToolPanelUnderCanvas != null)
            {
                // Make sure the mouse do not overlap with the panel
                tipToolPanelUnderCanvas.transform.position = new Vector3(
                    Input.mousePosition.x + tipToolPanelUnderCanvas.rectTransform.sizeDelta.x / 2 + cursorSize.x, 
                    Input.mousePosition.y - tipToolPanelUnderCanvas.rectTransform.sizeDelta.y / 2 - cursorSize.y, 
                    0f);
                // TODO make panel to the left of cursor when exceed screen
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayOnPinterEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisplayOnPinterExit(eventData);
    }

    protected virtual void DisplayOnPinterEnter(PointerEventData eventData)
    {
        tipToolObj.SetActive(true);
    }

    protected virtual void DisplayOnPinterExit(PointerEventData eventData)
    {
        tipToolObj.SetActive(false);
        tipToolObj.transform.position = new Vector3(2000, 2000, 0);
    }
}

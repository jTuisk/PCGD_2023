using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipToolSizeFitter : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _TextMeshPro;
    public TMPro.TextMeshProUGUI TextMeshPro
    {
        get
        {
            if(_TextMeshPro == null && this.GetComponentInChildren<TMPro.TextMeshProUGUI>())
            {
                _TextMeshPro = this.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            }
            return _TextMeshPro;
        }
    }

    private RectTransform _RectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if(_RectTransform == null)
            {
                _RectTransform = GetComponent<RectTransform>();
            }
            return _RectTransform;
        }
    }

    private float _preferredHeight;
    public float preferredHeight
    {
        get { return _preferredHeight; }
    }

    private float _preferredWidth;
    public float preferredWidth
    {
        get { return _preferredWidth; }
    }

    [SerializeField] float maxTipToolWidth = 500f;

    private void UpdateSize()
    {
        if(TextMeshPro == null)
            return;
        
        // TODO limit width
        
        _preferredWidth = TextMeshPro.preferredWidth;
        _preferredHeight = TextMeshPro.preferredHeight;
        rectTransform.sizeDelta = new Vector2(_preferredWidth, _preferredHeight);
    }

    private void Start()
    {
        UpdateSize();
    }

    private void Update()
    {
        
    }
}

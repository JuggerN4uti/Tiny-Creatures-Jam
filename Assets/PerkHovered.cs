using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerkHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string effectText;
    public TMPro.TextMeshProUGUI Tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.text = effectText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.text = "";
    }
}

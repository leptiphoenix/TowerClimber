using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class creditHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject creditpanel;
    public void OnPointerEnter(PointerEventData eventData)
    {
        creditpanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        creditpanel.SetActive(false);
    }
}

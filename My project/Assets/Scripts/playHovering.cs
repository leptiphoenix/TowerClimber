using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class playHovering : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] menu menuscript;
    public void OnPointerEnter(PointerEventData eventData)
    {
        menuscript.playhover();
    }
}

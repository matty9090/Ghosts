using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Serializable]
public struct WeaponUIEl {
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Texture2D img;
}

public class WeaponUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerEnter(PointerEventData eventData) {
        transform.GetChild(1).GetComponent<Image>().color = new Color(170.0f / 255.0f, 170.0f / 255.0f, 170.0f / 255.0f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.GetChild(1).GetComponent<Image>().color = new Color(124.0f / 255.0f, 124.0f / 255.0f, 124.0f / 255.0f);
    }
}

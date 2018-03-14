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
    public Sprite img;

    [SerializeField]
    public String str;
}

public class WeaponUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    public WeaponUIEl el;

    public void OnPointerDown(PointerEventData eventData) {
        GameObject.Find("Game").GetComponent<GameController>().CurrentWorm.GetComponent<WormMovement>().missile = el.prefab;

        transform.GetChild(2).position += new Vector3(0.0f, -3.0f, 0.0f);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.GetChild(1).GetComponent<Image>().color = new Color(170.0f / 255.0f, 170.0f / 255.0f, 170.0f / 255.0f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.GetChild(1).GetComponent<Image>().color = new Color(124.0f / 255.0f, 124.0f / 255.0f, 124.0f / 255.0f);
    }

    public void OnPointerUp(PointerEventData eventData) {
        GameObject.Find("Game").GetComponent<GameController>().hideUI();
        transform.GetChild(2).position += new Vector3(0.0f, 3.0f, 0.0f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    [SerializeField]
    GameObject ui;

    public void OnPointerDown(PointerEventData eventData) {
        
    }

    public void OnPointerUp(PointerEventData eventData) {
        GameObject.Find("Game").GetComponent<GameController>().showUI();
    }
}

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberController : MonoBehaviour {
    private static DamageNumbers popupText;
    private static GameObject canvas;

    public static void initialize() {
        canvas = GameObject.Find("UI");

        if(!popupText)
        {
            Debug.Log("HIIIIIIIIIIIII");
            popupText = Resources.Load<DamageNumbers>("Prefabs/PopupTextParent");
        }
    }

	public static void CreateFloatingText(string text, Transform location) {
        DamageNumbers instance = Instantiate(popupText);
        Vector2 screenPostion = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPostion;
        instance.SetText(text);
    }
}

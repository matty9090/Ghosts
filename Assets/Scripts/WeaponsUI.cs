using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour {
    [SerializeField]
    GameObject uiElement;

    [SerializeField]
    Transform listElement;

    [SerializeField]
    WeaponUIEl[] weapons;

	void Start () {
        Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

        foreach (WeaponUIEl el in weapons) {
            GameObject e = Instantiate(uiElement);
            e.transform.SetParent(listElement);
            e.transform.position = offset;
            e.transform.GetChild(2).GetComponent<Image>().sprite = el.img;
            e.GetComponent<WeaponUIElement>().el = el;

            offset += new Vector3(122.0f, 0.0f, 0.0f);
        }
	}
}

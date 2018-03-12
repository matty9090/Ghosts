using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour {
    public Animator animator;
    private Text damageText;

	void Start () {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
	}

    public void SetText(string text) {
        animator.GetComponent<Text>().text = text;
    }
}

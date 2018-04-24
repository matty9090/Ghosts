using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMenuWalk : MonoBehaviour {
    private SpriteRenderer sr;
    private Animator ani;
    private int Direction;
    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        ani.SetInteger("State", 1);
        Direction = -1;
    }
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x <= -50.0f && !sr.flipX)
        {
            sr.flipX = !sr.flipX;
            Direction = 1;
        }

        if (transform.position.x >= 50.0f && sr.flipX)
        {
            sr.flipX = !sr.flipX;
            Direction = -1;
        }

        transform.position = transform.position + new Vector3(0.2f * Direction , 0.0f, 0.0f);
    }
}

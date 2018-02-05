using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator ani;
    private Vector2 velocity = Vector2.zero;
    private int facing = -1;
    private bool isGrounded = true;

    // Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isGrounded == true)
        {
            velocity = new Vector2(0, rb.velocity.y);
            ani.SetInteger("State", 0);
        }
        else
        {
            velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        

        if (Input.GetKey(KeyCode.Space))
        {
            if(isGrounded == true)
            {
                isGrounded = false;
                velocity.x = 1f * facing;
                velocity.y = 5f;
                ani.SetInteger("State", 2);
            }
            
        }
        if(isGrounded == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                sr.flipX = false;
                velocity.x = -1;
                facing = -1;
                ani.SetInteger("State", 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                sr.flipX = true;
                velocity.x = 1;
                facing = 1;
                ani.SetInteger("State", 1);
            }

        }
       
       
        rb.velocity = velocity;
        
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
       if (coll.gameObject.tag == "Ground")
            isGrounded = true;

    }

}




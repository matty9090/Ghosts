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
    private int maxRotation = 90;
    private int minRotation = -90;
    private int currentRotation = 0;
    private int RotationSpeed = 1;

    public GameObject crosshair;
    public GameObject TestMissle;

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
                if (sr.flipX == true)
                {
                    sr.flipX = false;
                    crosshair.transform.RotateAround(transform.position, Vector3.forward, 180 - 2*(currentRotation));
                    
                }
                velocity.x = -1;
                facing = -1;
                ani.SetInteger("State", 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (sr.flipX == false)
                {
                    sr.flipX = true;
                    crosshair.transform.RotateAround(transform.position, Vector3.back, 180 - 2*(currentRotation));
                }
                velocity.x = 1;
                facing = 1;
                ani.SetInteger("State", 1);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                if (currentRotation < maxRotation)
                {
                    if (sr.flipX == false)
                    {
                        currentRotation += RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.back, RotationSpeed);
                    }
                    else
                    {
                        currentRotation += RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.forward, RotationSpeed);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (currentRotation > minRotation)
                {
                    if (sr.flipX == false)
                    {
                        currentRotation -= RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.forward, RotationSpeed);
                    }
                    else
                    {
                        currentRotation -= RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.back, RotationSpeed);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 tmp = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y,0.0f);
                Vector2 fromPlayerToCross = crosshair.transform.position - transform.position;
                var obj = (GameObject)Instantiate(TestMissle, tmp, Quaternion.LookRotation(fromPlayerToCross));
                obj.GetComponent<Rigidbody2D>().velocity = fromPlayerToCross * 10;
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




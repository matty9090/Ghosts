using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WormMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator ani;
    private Vector2 velocity = Vector2.zero;

    private int facing = -1;
    private bool isGrounded = true;
    private int maxRotation = 89;
    private int minRotation = -89;
    private int currentRotation = 0;
    private int RotationSpeed = 1;
    private int health = 100;

    public Image healthBar;
    public GameObject crosshair;
    public GameObject TestMissle;

    public enum WormState { Idle, Playing, Knockback };
    public WormState wormState;

    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        wormState = WormState.Idle;
	}
	
    void stateIdle() {
        if (isGrounded) {
            velocity = new Vector2(0, rb.velocity.y);
            ani.SetInteger("State", 0);
        } else
            velocity = new Vector2(rb.velocity.x, rb.velocity.y);
    }

    void stateKnockback() {
        // if (isGrounded)
        //    wormState = WormState.Idle;

        velocity = new Vector2(rb.velocity.x, rb.velocity.y);
    }

    void statePlaying() {
        if (isGrounded) {
            velocity = new Vector2(0, rb.velocity.y);
            ani.SetInteger("State", 0);
        } else
            velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (Input.GetKey(KeyCode.Space)) {
            if (isGrounded) {
                isGrounded = false;
                velocity.x = 1f * facing;
                velocity.y = 5f;
                ani.SetInteger("State", 2);
            }
        }

        if (isGrounded) {
            if (Input.GetKey(KeyCode.A)) {
                if (sr.flipX) {
                    sr.flipX = false;
                    crosshair.transform.RotateAround(transform.position, Vector3.forward, 180 - 2 * (currentRotation));
                }

                velocity.x = -1;
                facing = -1;
                ani.SetInteger("State", 1);
            } else if (Input.GetKey(KeyCode.D)) {
                if (!sr.flipX) {
                    sr.flipX = true;
                    crosshair.transform.RotateAround(transform.position, Vector3.back, 180 - 2 * (currentRotation));
                }

                velocity.x = 1;
                facing = 1;
                ani.SetInteger("State", 1);
            } else if (Input.GetKey(KeyCode.W)) {
                if (currentRotation < maxRotation) {
                    if (!sr.flipX) {
                        currentRotation += RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.back, RotationSpeed);
                    } else {
                        currentRotation += RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.forward, RotationSpeed);
                    }
                }
            } else if (Input.GetKey(KeyCode.S)) {
                if (currentRotation > minRotation) {
                    if (!sr.flipX) {
                        currentRotation -= RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.forward, RotationSpeed);
                    } else {
                        currentRotation -= RotationSpeed;
                        crosshair.transform.RotateAround(transform.position, Vector3.back, RotationSpeed);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                Vector3 tmp = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, 0.0f);
                Vector2 fromPlayerToCross = crosshair.transform.position - transform.position;
                var obj = (GameObject)Instantiate(TestMissle, tmp, Quaternion.LookRotation(fromPlayerToCross));
                obj.GetComponent<Rigidbody2D>().velocity = fromPlayerToCross * 10;

                GameObject.Find("Game").GetComponent<GameController>().Timer = 10.9f;
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        switch(wormState) {
            case WormState.Idle: stateIdle(); break;
            case WormState.Playing: statePlaying(); break;
            case WormState.Knockback: stateKnockback(); break;
        }

        rb.velocity = velocity;
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
            isGrounded = true;
    }

    public bool takeDamage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = (float)health / 100.0f;

        if (health < 0)
            return true;

        return false;
    }
}




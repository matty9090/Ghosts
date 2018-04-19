using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WormMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator ani;
    private Vector2 velocity = Vector2.zero;
    private GameController gameController;

    private int facing = -1;
    private bool isGrounded = true;
    private int currentRotation = 0;
    private float crosshairMoveSpeed = 0.05f;
    private int health = 100;

    private float knockTimer = 0.0f;
    private float deathFloor = -5.74f;

    public float knockbackTimer = 2.0f;
    public Image healthBar;
    public string wormName;
    public Text wormNametxt;
    public GameObject crosshair;
    public GameObject missile;

    public enum WormState { Idle, Playing, Knockback };
    public WormState wormState;

    // Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        gameController = GameObject.Find("Game").GetComponent<GameController>();
        wormState = WormState.Idle;
        wormNametxt.text = wormName;
        knockTimer = knockbackTimer;
    }
	
    void stateIdle() {
        crosshair.GetComponent<SpriteRenderer>().enabled = false;

        if (isGrounded) {
            velocity = new Vector2(0, rb.velocity.y);
            ani.SetInteger("State", 0);
        } else
            velocity = new Vector2(rb.velocity.x, rb.velocity.y);
    }

    void stateKnockback() {
        crosshair.GetComponent<SpriteRenderer>().enabled = false;

        knockTimer -= Time.deltaTime;

        if (knockTimer <= 0) {
            wormState = WormState.Idle;
            knockTimer = knockbackTimer;
        }

        velocity = new Vector2(rb.velocity.x, rb.velocity.y);
    }

    void statePlaying() {
        crosshair.GetComponent<SpriteRenderer>().enabled = true;

        if (isGrounded) {
            velocity = new Vector2(0, rb.velocity.y);
            ani.SetInteger("State", 0);
        } else
            velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (gameController.gameState == GameController.GameStates.Playing) {
            if (Input.GetKey(KeyCode.Space)) {
                if (isGrounded) {
                    isGrounded = false;
                    velocity.x = 1f * facing;
                    velocity.y = 5f;
                    ani.SetInteger("State", 2);
                }
            }

            if (isGrounded)
                missile.GetComponent<Crosshair>().control(crosshair, crosshairMoveSpeed, sr, ref currentRotation, transform.position);

            if (missile.GetComponent<Crosshair>().canMove()) {
                if (Input.GetKey(KeyCode.A)) {
                    if (sr.flipX) {
                        sr.flipX = false;
                    }

                    velocity.x = -1;
                    facing = -1;
                    ani.SetInteger("State", 1);
                } else if (Input.GetKey(KeyCode.D)) {
                    if (!sr.flipX) {
                        sr.flipX = true;
                    }

                    velocity.x = 1;
                    facing = 1;
                    ani.SetInteger("State", 1);
                }
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

        checkFallen();
        rb.velocity = velocity;
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Player")
            isGrounded = true;
    }

    void checkFallen()
    {
        if (transform.position.y <= deathFloor)
        {
            GameObject.Find("Game").GetComponent<GameController>().removeWorm(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public bool takeDamage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = (float)health / 100.0f;
        DamageNumberController.CreateFloatingText(amount.ToString(), transform);

        ani.SetInteger("State", 3);

        if (health < 0)
            return true;

        return false;
    }

    public void setTextColour(Color color)
    {
        wormNametxt.color = color;
    }

    public void SwapToCrosshair()
    {
        if (sr.flipX)
            crosshair.transform.position = new Vector3(0.704f, 0.0f, 0.0f) + transform.position;
        else
            crosshair.transform.position = new Vector3(-0.704f, 0.0f, 0.0f) + transform.position;

        currentRotation = 0;
    } 
}
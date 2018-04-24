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
    private bool canDoubleJump = true;
    private int currentRotation = 0;
    private float crosshairMoveSpeed = 0.2f;
    private int health = 100;

    private float knockTimer    = 0.0f;
    private float deathFloor    = -5.74f;
    private float ceiling       = 4.2f;
    private float flyingTimer   = 2.0f;

    public float knockbackTimer = 2.0f;
    public Image healthBar;
    public string wormName;
    public Text wormNametxt;
    public GameObject crosshair;
    public GameObject missile;
    public GameObject uiHealth;

    public enum WormState { Idle, Playing, Knockback };
    public WormState wormState;

    [SerializeField]
    public AudioSource damageSound;

    // Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        gameController = GameObject.Find("Game").GetComponent<GameController>();
        wormState = WormState.Idle;
        wormNametxt.text = wormName;
        knockTimer = knockbackTimer;

        uiHealth.transform.GetChild(0).GetComponent<Text>().text = wormName;
        uiHealth.transform.GetChild(1).GetComponent<Text>().text = wormName;
        uiHealth.transform.GetChild(3).GetComponent<Image>().enabled = false;
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
        if(gameController.canFire)
            crosshair.GetComponent<SpriteRenderer>().enabled = true;

        if (isGrounded) {
            velocity = new Vector2(0, rb.velocity.y);
            ani.SetInteger("State", 0);
            flyingTimer = 2.0f;
        } else {
            velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            flyingTimer -= Time.deltaTime;
        }

        if (gameController.gameState == GameController.GameStates.Playing) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (isGrounded) {
                    isGrounded = false;
                    velocity.x = 1.0f * facing;
                    velocity.y = 4.4f;
                    ani.SetInteger("State", 2);
                }
                else if(canDoubleJump)
                {
                    canDoubleJump = false;
                    velocity.x = 1f * facing;
                    velocity.y = 4f;
                    ani.SetInteger("State", 2);
                }
            }

            if (isGrounded && gameController.canFire)
                missile.GetComponent<Crosshair>().control(crosshair, crosshairMoveSpeed, sr, ref currentRotation, transform.position);

            if (missile.GetComponent<Crosshair>().canMove() && isGrounded || !gameController.canFire) {
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
        
        if( flyingTimer < 0.0f)
        {
            isGrounded = true;
            canDoubleJump = true;
        }

        checkFallen();
        checkCeiling();

        rb.velocity = velocity;
    }

    private void OnDrawGizmos() {
        float y = GetComponent<CapsuleCollider2D>().offset.y + GetComponent<CapsuleCollider2D>().size.y - 0.08f;
        Ray r = new Ray(transform.position - new Vector3(0.0f, y, 0.0f), Vector3.down);
        Gizmos.DrawRay(r);

        float x = GetComponent<CapsuleCollider2D>().size.x / 2.0f;
        y = GetComponent<CapsuleCollider2D>().offset.y + GetComponent<CapsuleCollider2D>().size.y - 0.09f;

        r = new Ray(transform.position - new Vector3(sr.flipX ? -x : x, y, 0.0f), new Vector2(sr.flipX ? 1.0f : -1.0f, 0.0f));
        Gizmos.DrawRay(r);
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
        float y_off = GetComponent<CapsuleCollider2D>().offset.y + GetComponent<CapsuleCollider2D>().size.y - 0.08f;
        RaycastHit2D d_hit = Physics2D.Raycast(transform.position - new Vector3(0.0f, y_off, 0.0f), Vector3.down, 0.08f);

        if (d_hit.collider != null) {
            isGrounded = true;
            canDoubleJump = true;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                float x = GetComponent<CapsuleCollider2D>().size.x / 2.0f;
                float y = GetComponent<CapsuleCollider2D>().offset.y + GetComponent<CapsuleCollider2D>().size.y - 0.09f;

                RaycastHit2D s_hit = Physics2D.Raycast(transform.position - new Vector3(sr.flipX ? -x : x, y, 0.0f), new Vector2(sr.flipX ? 1.0f : -1.0f, 0.0f), 0.14f);

                if (s_hit.collider != null) {
                    transform.Translate(sr.flipX ? 0.064f : -0.064f, (s_hit.transform.position.y - d_hit.transform.position.y) + 0.02f, 0.0f);
                }
            }
        }
    }

    void checkCeiling()
    {
        if (transform.position.y >= ceiling)
            transform.position = new Vector3(transform.position.x, ceiling, transform.position.z);
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
        uiHealth.transform.GetChild(2).GetComponent<Image>().fillAmount = (float)health / 100.0f;

        DamageNumberController.CreateFloatingText(amount.ToString(), transform);

        ani.SetInteger("State", 3);

        if (health <= 0) {
            uiHealth.transform.GetChild(3).GetComponent<Image>().enabled = true;

            return true;
        }

        damageSound.Play();

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

        crosshair.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        currentRotation = 0;
    }
}
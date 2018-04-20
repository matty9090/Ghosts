using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour, Crosshair {
    [SerializeField]
    float explosionRadius;

    [SerializeField]
    int rocketDamage;

    [SerializeField]
    int bounceCount;


    [SerializeField]
    GameObject explosion;

    bool collided;
    int currentBounceCount;
    float hitTimer;
    const float MAXTIMER = 0.05f;
    Vector3 vel;

    private int maxRotation = 89;
    private int minRotation = -89;
    private int RotationSpeed = 1;

    private void Start()
    {
        hitTimer = 0;
        collided = false;
        currentBounceCount = 0;
    }

    private void Update()
    {
        vel = GetComponent<Rigidbody2D>().velocity;
        vel.Normalize();
        vel *= 5;

        if (vel.x == 0 && vel.y == 0)
            vel = new Vector2(0.1f, 0.1f);

        GetComponent<Rigidbody2D>().velocity = vel;
        transform.rotation = Quaternion.LookRotation(-vel);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, transform.rotation.z));
        hitTimer -= Time.deltaTime;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!collided && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player"))
        {
           GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, explosionRadius);

            List<GameObject> worms = GameObject.Find("Game").GetComponent<GameController>().getAllWorms();

            for (int i = worms.Count - 1; i >= 0; i--)
            {
                GameObject worm = worms[i];

                float dx = worm.transform.position.x - collision.collider.transform.position.x;
                float dy = worm.transform.position.y - collision.collider.transform.position.y;

                Vector2 dir = (new Vector2(dx, dy)).normalized;
                Vector2 force = new Vector2((explosionRadius - Mathf.Abs(dx)) * dir.x * 300.0f, (explosionRadius - Mathf.Abs(dy)) * dir.y * 300.0f);

                if (dx * dx + dy * dy < explosionRadius * explosionRadius)
                {
                    if (!worm.GetComponent<WormMovement>().takeDamage(rocketDamage))
                    {
                        worm.GetComponent<Rigidbody2D>().AddForce(force);
                        worm.GetComponent<WormMovement>().wormState = WormMovement.WormState.Knockback;
                    }
                    else
                    {
                        worms.RemoveAt(i);
                        GameObject.Find("Game").GetComponent<GameController>().removeWorm(worm);
                        Destroy(worm);
                    }
                }
            }

            if(hitTimer <= 0)
            {
                hitTimer = MAXTIMER;
                currentBounceCount++;
            }

            if(currentBounceCount >= bounceCount)
            {
                GameObject expl = Instantiate(explosion, collision.collider.transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(expl, 0.5f);
                Destroy(gameObject);
            }
        }
    }

    public bool canMove()
    {
        return true;
    }

    public void control(GameObject crosshair, float speed, SpriteRenderer sr, ref int rotation, Vector3 pos)
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (sr.flipX)
                crosshair.transform.RotateAround(pos, Vector3.forward, 180 - 2 * (rotation));

        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!sr.flipX)
                crosshair.transform.RotateAround(pos, Vector3.back, 180 - 2 * (rotation));

        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (rotation < maxRotation)
            {
                if (!sr.flipX)
                {
                    rotation += RotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.back, RotationSpeed);
                }
                else
                {
                    rotation += RotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.forward, RotationSpeed);
                }
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (rotation > minRotation)
            {
                if (!sr.flipX)
                {
                    rotation -= RotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.forward, RotationSpeed);
                }
                else
                {
                    rotation -= RotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.back, RotationSpeed);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameController game = GameObject.Find("Game").GetComponent<GameController>();

            Vector3 tmp = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, 0.0f);
            Vector2 fromPlayerToCross = crosshair.transform.position - pos;

            if (game.canFire) {
                game.Timer = 10.9f;
                var obj = (GameObject)Instantiate(gameObject, tmp, Quaternion.LookRotation(fromPlayerToCross));
                obj.GetComponent<Rigidbody2D>().velocity = fromPlayerToCross * 10;
            }

            game.canFire = false;
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}

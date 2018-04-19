using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerBomb : MonoBehaviour, Crosshair
{
    [SerializeField]
    float explosionRadius;

    [SerializeField]
    float digRadius;

    [SerializeField]
    float digTime;

    [SerializeField]
    int rocketDamage;
  
    [SerializeField]
    GameObject explosion;

    bool collided;
    bool timerStart;
    float timer;

    [SerializeField]
    Vector3 vel;


    private void Start()
    {
        collided = false;
        timerStart = false;
        timer = digTime;
        GetComponent<Rigidbody2D>().velocity = vel;
    }

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = vel;
        transform.rotation = Quaternion.LookRotation(-vel);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, transform.rotation.z));

        if (timerStart)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, digRadius);
            timerStart = true;
        }

        if (!collided && ( collision.gameObject.tag == "Player" || timer <= 0))
        {
            GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, explosionRadius);
            GameObject expl = Instantiate(explosion, collision.collider.transform.position, Quaternion.Euler(0, 0, 0));

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

            Destroy(expl, 0.5f);

            collided = true;
            Destroy(gameObject);
        }
    }

    public bool canMove()
    {
        return false;
    }

    public void control(GameObject crosshair, float speed, SpriteRenderer sr, ref int rotation, Vector3 pos)
    {
        if (Input.GetKey(KeyCode.A))
        {
            crosshair.transform.Translate(new Vector3(-speed, 0.0f, 0.0f));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            crosshair.transform.Translate(new Vector3(speed, 0.0f, 0.0f));
        }
        if (Input.GetKey(KeyCode.W))
        {
            crosshair.transform.Translate(new Vector3(0.0f, speed, 0.0f));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            crosshair.transform.Translate(new Vector3(0.0f, -speed, 0.0f));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject.Find("Game").GetComponent<GameController>().Timer = 10.9f;
            Vector3 tmp = new Vector3(crosshair.transform.position.x, 6.0f, 0.0f);
            Vector2 crosshairPosition = crosshair.transform.position - tmp;

            Instantiate(gameObject, tmp, Quaternion.LookRotation(crosshairPosition));
        }
    }
}

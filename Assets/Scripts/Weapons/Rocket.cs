using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, Crosshair {
    [SerializeField]
    float explosionRadius;

    [SerializeField]
    int rocketDamage;

    [SerializeField]
    GameObject explosion;

    bool collided;
    Vector3 vel;
    AudioSource audioCast;

    private int maxRotation = 89;
    private int minRotation = -89;
    private int rotationSpeed = 2;

    private void Start() {
        collided = false;
        audioCast = GetComponent<AudioSource>();
    }

    private void Update() {
        vel = GetComponent<Rigidbody2D>().velocity;
        transform.rotation = Quaternion.LookRotation(-vel);
        transform.Rotate(new Vector2(0, 90));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")) {
            GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, explosionRadius);
            GameObject expl = Instantiate(explosion, collision.collider.transform.position, Quaternion.Euler(0, 0, 0));

            List<GameObject> worms = GameObject.Find("Game").GetComponent<GameController>().getAllWorms();

            for(int i = worms.Count - 1; i >= 0; i--) {
                GameObject worm = worms[i];

                float dx = worm.transform.position.x - collision.collider.transform.position.x;
                float dy = worm.transform.position.y - collision.collider.transform.position.y;

                Vector2 dir   = (new Vector2(dx, dy)).normalized;
                Vector2 force = new Vector2((explosionRadius - Mathf.Abs(dx)) * dir.x * 300.0f, (explosionRadius - Mathf.Abs(dy)) * dir.y * 300.0f);

                if (dx * dx + dy * dy < explosionRadius * explosionRadius) {
                    if (!worm.GetComponent<WormMovement>().takeDamage(rocketDamage)) {
                        worm.GetComponent<Rigidbody2D>().AddForce(force);
                        worm.GetComponent<WormMovement>().wormState = WormMovement.WormState.Knockback;
                    } else {
                        worms.RemoveAt(i);
                        GameObject.Find("Game").GetComponent<GameController>().removeWorm(worm);
                        Destroy(worm);
                    }
                }
            }

            Destroy(expl, 0.7f);

            collided = true;
            Destroy(gameObject);
        }
    }

    public bool canMove() {
        return true;
    }

    public void control(GameObject crosshair, float speed, SpriteRenderer sr, ref int rotation, Vector3 pos) {
        if (Input.GetKey(KeyCode.A)) {
            if (sr.flipX)
                crosshair.transform.RotateAround(pos, Vector3.forward, 180 - 2 * (rotation));

        } else if (Input.GetKey(KeyCode.D)) {
            if (!sr.flipX)
                crosshair.transform.RotateAround(pos, Vector3.back, 180 - 2 * (rotation));
            
        } else if (Input.GetKey(KeyCode.W)) {
            if (rotation < maxRotation) {
                if (!sr.flipX) {
                    rotation += rotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.back, rotationSpeed);
                } else {
                    rotation += rotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.forward, rotationSpeed);
                }
            }
        } else if (Input.GetKey(KeyCode.S)) {
            if (rotation > minRotation) {
                if (!sr.flipX) {
                    rotation -= rotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.forward, rotationSpeed);
                } else {
                    rotation -= rotationSpeed;
                    crosshair.transform.RotateAround(pos, Vector3.back, rotationSpeed);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            GameController game = GameObject.Find("Game").GetComponent<GameController>();

            Vector3 tmp = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, 0.0f);
            Vector2 fromPlayerToCross = crosshair.transform.position - pos;

            if (game.canFire) {
                var obj = (GameObject)Instantiate(gameObject, tmp, Quaternion.LookRotation(fromPlayerToCross));
                obj.GetComponent<Rigidbody2D>().velocity = fromPlayerToCross * 10;

                if (game.Timer > 10.9f)
                    game.Timer = 10.9f;
            }

            game.canFire = false;
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}

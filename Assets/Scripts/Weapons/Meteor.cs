using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour, Crosshair {
    [SerializeField]
    float explosionRadius;

    [SerializeField]
    int rocketDamage;

    [SerializeField]
    GameObject meteorite;

    [SerializeField]
    int meteoriteCount;

    [SerializeField]
    GameObject explosion;

    bool collided;
    bool explode;
    Vector3 vel;
    Vector3 targetPosition;

    private void Start() {
        collided = false;
        explode = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.5f);
    }

    private void Update() {
        vel = GetComponent<Rigidbody2D>().velocity;
        transform.rotation = Quaternion.LookRotation(-vel);
        transform.Rotate(new Vector3(90, 0, 0));

        if ((transform.position - targetPosition).magnitude < 1.0f)
            GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        

        if (!collided && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")) {
            GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, explosionRadius);
            GameObject expl = Instantiate(explosion, collision.collider.transform.position, Quaternion.Euler(0, 0, 0));

            List<GameObject> worms = GameObject.Find("Game").GetComponent<GameController>().getAllWorms();

            for (int i = worms.Count - 1; i >= 0; i--) {
                GameObject worm = worms[i];

                float dx = worm.transform.position.x - collision.collider.transform.position.x;
                float dy = worm.transform.position.y - collision.collider.transform.position.y;

                Vector2 dir = (new Vector2(dx, dy)).normalized;
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

           GameObject.Find("Main Camera").GetComponent<PositionCamera>().pan();

            createMeteorites();
            Destroy(expl, 0.5f);

            collided = true;
            Destroy(gameObject);
        }
    }

    void createMeteorites() {
        for (int counter = 0; counter < meteoriteCount; counter++) {
            float xdir = Random.Range(-3.0f, 3.0f);
            float ydir = Random.Range(3.0f, 6.0f);
            Vector3 Direction = new Vector3(xdir, ydir, 0.0f);
            var obj = (GameObject)Instantiate(meteorite, this.transform.position, Quaternion.LookRotation(Direction));
            obj.GetComponent<Rigidbody2D>().velocity = Direction;
        }
    }

    public void getTargetPos(Vector3 target)
    {
        targetPosition = target;
    }

    public bool canMove() {
        return false;
    }

    public void control(GameObject crosshair, float speed, SpriteRenderer sr, ref int rotation, Vector3 pos) {
        if (Input.GetKey(KeyCode.A)) {
            crosshair.transform.Translate(new Vector3(-speed, 0.0f, 0.0f));
        } else if (Input.GetKey(KeyCode.D)) {
            crosshair.transform.Translate(new Vector3(speed, 0.0f, 0.0f));
        }
        if (Input.GetKey(KeyCode.W)) {
            crosshair.transform.Translate(new Vector3(0.0f, speed, 0.0f));
        } else if (Input.GetKey(KeyCode.S)) {
            crosshair.transform.Translate(new Vector3(0.0f, -speed, 0.0f));
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            GameController game = GameObject.Find("Game").GetComponent<GameController>();

            Vector3 tmp = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y + 10.0f, 0.0f);
            Vector2 crosshairPosition = crosshair.transform.position - tmp;

            if (game.canFire) {
                GameObject meteor = Instantiate(gameObject, tmp, Quaternion.LookRotation(crosshairPosition));
                meteor.GetComponent<Meteor>().getTargetPos(crosshair.transform.position);
                GameObject.Find("Main Camera").GetComponent<PositionCamera>().trackCrosshair();

                if (game.Timer > 10.9f)
                    game.Timer = 10.9f;
            }

            game.canFire = false;
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}

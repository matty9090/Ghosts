using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    [SerializeField]
    float explosionRadius;

    bool collided;

	private void Start () {
        collided = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 10.0f);
    }

    private void Update() {
        Vector3 vel = GetComponent<Rigidbody2D>().velocity;
        transform.rotation = Quaternion.LookRotation(-vel);
        transform.Rotate(new Vector2(0, 90));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!collided && collision.gameObject.tag == "Ground") {
            GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, explosionRadius);
            collided = true;
            Destroy(this.gameObject);
        }
    }
}

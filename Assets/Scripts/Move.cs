using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	private void Start () {
        GetComponent<Rigidbody>().velocity = new Vector3(9.0f, 10.0f, 0.0f);
    }

    private void Update() {
        Vector3 vel = GetComponent<Rigidbody>().velocity;
        transform.rotation = Quaternion.LookRotation(vel);
        transform.Rotate(new Vector3(-90, 0, 0));
    }
}

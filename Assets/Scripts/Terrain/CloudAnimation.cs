using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnimation : MonoBehaviour {
    [SerializeField]
    float XSpeed;

    [SerializeField]
    float Xbegin;

    [SerializeField]
    float Xend;

    [SerializeField]
    float Ymax;

    [SerializeField]
    float Ymin;

    [SerializeField]
    float YSpeed;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.x > Xend)
            transform.position = new Vector3(Xbegin, transform.position.y, transform.position.z);

        transform.position = new Vector3(transform.position.x + XSpeed, transform.position.y, transform.position.z);
    }
}

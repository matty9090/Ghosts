using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCamera : MonoBehaviour {
    [SerializeField]
    GameController controller;

    [SerializeField]
    float sceneStart;

    [SerializeField]
    float sceneEnd;

    void Update() {
        Vector3 pos = new Vector3(controller.CurrentWorm.transform.position.x, transform.position.y, transform.position.z);

        if(pos.x >= sceneStart && pos.x <= sceneEnd)
            transform.position = pos;
    }
}

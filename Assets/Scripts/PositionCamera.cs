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

    public bool panned = false;

    enum CameraState { Tracking, Panning }
    CameraState cameraState;

    private void Awake() {
        cameraState = CameraState.Tracking;
    }

    public void pan() {
        cameraState = CameraState.Panning;
    }

    void Update() {
        if (GameObject.Find("Game").GetComponent<GameController>().gameState == GameController.GameStates.GameOver)
            return;

        switch(cameraState) {
            case CameraState.Tracking:
                Vector3 pos = new Vector3(controller.CurrentWorm.transform.position.x, transform.position.y, transform.position.z);

                if (pos.x >= sceneStart && pos.x <= sceneEnd)
                    transform.position = pos;

                break;

            case CameraState.Panning:
                float target = controller.CurrentWorm.transform.position.x;

                if (target < sceneStart) target = sceneStart;
                if (target > sceneEnd)   target = sceneEnd;

                int dir = ((controller.CurrentWorm.transform.position.x - transform.position.x) > 0) ? 1 : -1;
                float distance = Mathf.Abs(transform.position.x - target);
                float scalar = (Mathf.Pow(distance + 2.0f, 2)) / 4.0f;
                float x = transform.position.x + dir * Time.deltaTime * scalar;

                transform.position = new Vector3(x, transform.position.y, transform.position.z);

                if (distance < 0.12f) {
                    panned = true;
                    cameraState = CameraState.Tracking;
                }

                break;
        }        
    }
}

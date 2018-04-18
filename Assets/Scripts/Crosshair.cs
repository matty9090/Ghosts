using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Crosshair {
    bool canMove();
    void control(GameObject crosshair, float speed, SpriteRenderer sr, ref int rotation, Vector3 pos);
}

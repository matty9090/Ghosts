using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour {
    [SerializeField]
    int terrainWidth = 513; // 2^n + 1

    [SerializeField]
    int sceneWidth = 20;

    [SerializeField]
    float height = 1.0f;

    [SerializeField]
    float roughness = 0.5f;

    [SerializeField]
    float y_offset = 0.0f;

    void Start () {
        MidPointDisplacement t = new MidPointDisplacement(terrainWidth, sceneWidth, height, roughness, y_offset);

        GetComponent<LineRenderer>().positionCount = t.terrainData.Length;
        GetComponent<LineRenderer>().SetPositions(t.terrainData);
	}
}

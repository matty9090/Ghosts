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

    [SerializeField]
    float voxel_size = 0.1f;

    [SerializeField]
    GameObject voxel;

    void Start () {
        MidPointDisplacement t = new MidPointDisplacement(terrainWidth, sceneWidth, height, roughness, y_offset);

        GetComponent<LineRenderer>().positionCount = t.terrainData.Length;
        GetComponent<LineRenderer>().SetPositions(t.terrainData);

        rasterize(t.terrainData);
	}

    void rasterize(Vector3[] data) {
        for (int i = 0; i < data.Length; i++) {
            for (float y = data[i].y + 0.5f; y > -5.0f; y -= 0.1f) {
                Instantiate(voxel, new Vector3(data[i].x, y, -1.0f), Quaternion.Euler(0, 0, 0));
            }
        }
    }
}

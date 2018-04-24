using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPerlin {
    private Dictionary<string, GameObject> voxels;

    public TerrainPerlin(GameObject voxel, float voxel_size, float x_offset, float y_offset, float scene_width, float ratio) {
        voxels = new Dictionary<string, GameObject>();

        float rx = Random.Range(-1000.0f, 1000.0f);
        float ry = Random.Range(-1000.0f, 1000.0f);

        for (float y = 5; y > -5.0f; y -= voxel_size) {
            for (float x = 0; x < scene_width; x += voxel_size) {
                if (Perlin.Noise((x + rx) / 2.5f, (y + ry) / 1.8f) > 0.06f) {
                    GameObject v = GameObject.Instantiate(voxel);
                    v.transform.position = new Vector3(x + x_offset, y + y_offset, 0.0f);

                    string index = Mathf.Round(x / voxel_size) + "," + Mathf.Round(y / voxel_size);
                    voxels[index] = v;
                }
            }
        }
    }

    public Dictionary<string, GameObject> Terrain {
        get {
            return voxels;
        }
    }
}

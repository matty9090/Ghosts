using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour {
    [SerializeField]
    int sceneWidth = 20;

    [SerializeField]
    float x_offset = -7.0f;

    [SerializeField]
    float y_offset = 0.0f;

    [SerializeField]
    float voxel_size = 0.1f;

    [SerializeField]
    GameObject voxel; 

    private Dictionary<string, GameObject> voxels;

    float ratio;
    int terrainWidth;

    void Awake() {
        terrainWidth = (int)((float)sceneWidth / voxel_size);
        ratio        = (float)sceneWidth / (float)terrainWidth;

        TerrainPerlin gen = new TerrainPerlin(voxel, voxel_size, x_offset, y_offset, sceneWidth, ratio);
        voxels = gen.Terrain;
    }

    public void removeVoxelsInRadius(Vector2 worldPos, float radius) {
        int sx = (int)((worldPos.x - x_offset - radius) / ratio);
        int ex = (int)((worldPos.x - x_offset + radius) / ratio);

        int sy = (int)((worldPos.y + y_offset - radius) / ratio);
        int ey = (int)((worldPos.y - y_offset + radius) / ratio);

        sx = (sx < 0) ? 0 : sx;
        sx = (sx >= terrainWidth - 1) ? terrainWidth - 1 : sx;

        ex = (ex < 0) ? 0 : ex;
        ex = (ex >= terrainWidth - 1) ? terrainWidth - 1 : ex;

        for (int x = sx; x <= ex; x++) {
            for (int y = sy; y <= ey; y++) {
                string index = x + "," + y;
                
                if (voxels.ContainsKey(index)) {
                    GameObject v = voxels[index];

                    float dx = v.transform.position.x - worldPos.x;
                    float dy = v.transform.position.y - worldPos.y;

                    dx *= dx;
                    dy *= dy;

                    if (dx + dy <= radius * radius) {
                        Destroy(v);
                        voxels.Remove(index);
                    }
                }
            }
        }
    }
}
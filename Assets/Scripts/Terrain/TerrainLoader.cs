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
    float x_offset = -7.0f;

    [SerializeField]
    float y_offset = 0.0f;

    [SerializeField]
    float voxel_size = 0.1f;

    [SerializeField]
    GameObject voxel;

    Dictionary<string, GameObject> voxels;
    Vector3[] terrain;
    EdgeCollider2D terrainCollider;
    LineRenderer lineRenderer;

    float ratio;

    void Start() {
        ratio = (float)sceneWidth / (float)terrainWidth;
        terrainCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        voxels = new Dictionary<string, GameObject>();

        MidPointDisplacement t = new MidPointDisplacement(terrainWidth, sceneWidth, height, roughness, x_offset, y_offset);
        terrain = t.terrainData;

        lineRenderer.positionCount = terrain.Length;
        lineRenderer.SetPositions(terrain);

        terrainCollider.points = t.terrainData2D;

        rasterize(t.terrainData);
    }

    void rasterize(Vector3[] data) {
        for (int i = 0; i < data.Length; i++) {
            for (float y = data[i].y - 0.12f; y > -4.2f; y -= voxel_size) {
                string index = i + "," + (int)(y / ratio);
                voxels[index] = Instantiate(voxel, new Vector3(data[i].x, y, -1.0f), Quaternion.Euler(0, 0, 0));
            }
        }
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

                    if (Mathf.Sqrt(dx + dy) <= radius) {
                        Destroy(v);
                        voxels.Remove(index);
                    }
                }
            }
        }

        lineRenderer.SetPositions(terrain);
    }
}
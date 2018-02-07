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

    struct VoxelCol {
        public List<GameObject> voxels;
    }

    List<VoxelCol> voxels;
    Vector3[] terrain;
    EdgeCollider2D terrainCollider;
    LineRenderer lineRenderer;

    float ratio;

    void Start() {
        ratio = (float)sceneWidth / (float)terrainWidth;
        terrainCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        voxels = new List<VoxelCol>();

        MidPointDisplacement t = new MidPointDisplacement(terrainWidth, sceneWidth, height, roughness, x_offset, y_offset);
        terrain = t.terrainData;

        lineRenderer.positionCount = terrain.Length;
        lineRenderer.SetPositions(terrain);

        terrainCollider.points = t.terrainData2D;

        rasterize(t.terrainData);
    }

    void rasterize(Vector3[] data) {
        for (int i = 0; i < data.Length; i++) {
            VoxelCol col = new VoxelCol();
            col.voxels = new List<GameObject>();

            for (float y = data[i].y - 0.12f; y > -4.2f; y -= 0.12f)
                col.voxels.Add(Instantiate(voxel, new Vector3(data[i].x, y, -1.0f), Quaternion.Euler(0, 0, 0)));

            voxels.Add(col);
        }
    }

    public void removeVoxelsInRadius(Vector2 worldPos, float radius) {
        foreach (VoxelCol c in voxels) {
            foreach (GameObject v in c.voxels) {
                float dx = v.transform.position.x - worldPos.x;
                float dy = v.transform.position.y - worldPos.y;

                dx *= dx;
                dy *= dy;

                if (Mathf.Sqrt(dx + dy) <= radius)
                    Destroy(v);
            }
        }

        lineRenderer.SetPositions(terrain);
    }
}
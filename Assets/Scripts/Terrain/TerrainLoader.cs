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
        public int top;
    }

    List<VoxelCol> voxels;
    Vector3[] terrain;
    EdgeCollider2D terrainCollider;
    LineRenderer lineRenderer;

    float ratio;

    void Start () {
        ratio           = (float)sceneWidth / (float)terrainWidth;
        terrainCollider = GetComponent<EdgeCollider2D>();
        lineRenderer    = GetComponent<LineRenderer>();
        voxels          = new List<VoxelCol>();

        MidPointDisplacement t = new MidPointDisplacement(terrainWidth, sceneWidth, height, roughness, x_offset, y_offset);
        terrain = t.terrainData;

        lineRenderer.positionCount = terrain.Length;
        lineRenderer.SetPositions(terrain);
        
        terrainCollider.points = t.terrainData2D;

        rasterize(t.terrainData);

        removeVoxelsInRadius(new Vector3(0, 0, 0), 3.0f);
	}

    void rasterize(Vector3[] data) {
        for (int i = 0; i < data.Length; i++) {
            VoxelCol col = new VoxelCol();
            col.voxels = new List<GameObject>();
            col.top = 0;

            for (float y = data[i].y - 0.12f; y > -4.2f; y -= 0.12f)
                col.voxels.Add(Instantiate(voxel, new Vector3(data[i].x, y, -1.0f), Quaternion.Euler(0, 0, 0)));

            voxels.Add(col);
        }
    }

    void removeVoxelsInRadius(Vector3 worldPos, float radius) {
        foreach(VoxelCol c in voxels) {
            int top = c.top;
            VoxelCol col = c;

            foreach (GameObject v in c.voxels) {
                if ((worldPos - v.transform.position).magnitude <= radius) {
                    Destroy(v);
                    top++;
                }
            }

            col.top = top;
        }

        for(int x = 0; x < terrain.Length; x++) {
            terrain[x].y = voxels[x].voxels[voxels[x].top].transform.position.y + 0.12f;
        }
        
        lineRenderer.SetPositions(terrain);
    }
}

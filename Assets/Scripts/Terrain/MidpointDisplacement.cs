using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    MidPoint Displacement Algorithm 
*/
public class MidPointDisplacement : TerrainGenerator {
    private float h;
    private float r;
    private int iterations;

    float[] terrain;

    public MidPointDisplacement(int width, int scene_width, float height, float roughness, float off) : base(width, scene_width, off) {
        h = height;
        r = roughness;
        iterations = (int)Mathf.Log((float)(width - 1), 2.0f);

        terrain = new float[width];

        generate();
    }
    
    public override void generate() {
        terrain[0]         = Random.Range(-h, h);
        terrain[width - 1] = Random.Range(-h, h);

        for (int i = 0; i < iterations; i++) {
            int k = (width - 1) / (int)Mathf.Pow(2.0f, (float)i);

            for (int j = 0; j <= width - k; j += k) {
                int jh = j + (k / 2);
                terrain[jh] = ((terrain[j] + terrain[j + k]) / 2.0f);
                terrain[j + (k / 2)] += Random.Range(-h, h);
            }

            h *= r;
        }

        for (int i = 0; i < width; i++) {
            terrainCoords[i] = new Vector3(i * ratio - 7.0f, terrain[i] + y_offset, 1.0f);
        }
    }

    public void display() {
        for(int i = 0; i < width; i++)
            Debug.Log(terrain[i]);
    }
}

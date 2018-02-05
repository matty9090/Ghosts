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

    public MidPointDisplacement(int width) : base(width) {
        h = 2.0f;
        r = 0.5f;
        iterations = 3;

        terrain = new float[width];

        generate();
        display();
    }
    
    public override void generate() {
        terrain[0]         = 0.0f;
        terrain[width - 1] = 0.0f;

        for (int i = 0; i < iterations; i++) {
            int k = (width - 1) / (int)Mathf.Pow(2.0f, (float)i);
            
            for (int j = 0; j <= width - k; j += k)
                terrain[j + (k / 2)] = (terrain[j] + terrain[j + (k / 2)] + Random.Range(-h, h));

            h *= r;
        }

        for (int i = 0; i < width; i++) {
            terrainCoords[i] = new Vector3(i * 3 - 7, terrain[i] * 2.0f, 1.0f);
        }
    }

    public void display() {
        for(int i = 0; i < width; i++)
            Debug.Log(terrain[i]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator {
    protected int width, scene_w;
    protected Vector3[] terrainCoords;
    protected float ratio;
    protected float y_offset;

    public TerrainGenerator(int w, int sw, float off) {
        width = w;
        scene_w = sw;
        y_offset = off;
        ratio = (float)sw / (float)w;
        terrainCoords = new Vector3[width];
    }

    public virtual void generate() {}
    public virtual Vector3[] terrainData { get { return terrainCoords; } }
}
